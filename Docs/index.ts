import { serve } from "https://deno.land/std@0.177.0/http/server.ts"
import { createClient } from 'https://esm.sh/@supabase/supabase-js@2.39.3'

serve(async () => {
  // Supabase automatically injects these environment variables into Edge Functions
  const supabaseUrl = Deno.env.get('SUPABASE_URL') ?? ''
  const supabaseKey = Deno.env.get('SUPABASE_SERVICE_ROLE_KEY') ?? ''
  const supabase = createClient(supabaseUrl, supabaseKey)

  try {
    // 1. Fetch the Heartbeat
    const { data: appState, error: stateError } = await supabase
      .from('app_state')
      .select('last_client_heartbeat')
      .eq('id', 1)
      .single()

    if (stateError) throw stateError

    // 2. Check if we should go to sleep
    const lastHeartbeat = new Date(appState.last_client_heartbeat)
    const now = new Date()
    const diffMinutes = (now.getTime() - lastHeartbeat.getTime()) / (1000 * 60)

    if (diffMinutes > 15) {
      console.log(`Last heartbeat was ${diffMinutes.toFixed(1)} mins ago. Sleeping.`)
      return new Response(
        JSON.stringify({ status: "sleeping", minutes_since_heartbeat: diffMinutes }),
        { headers: { "Content-Type": "application/json" } }
      )
    }

    console.log("Clients active! Fetching active tournaments...")

    // 3. Get ONLY the active or enabled tournaments
    const nowIso = now.toISOString()
    const { data: activeTournaments, error: tournamentsError } = await supabase
      .from('tournaments')
      .select('*')
      .or(`is_enabled.eq.true,and(start_date.lte.${nowIso},end_date.gte.${nowIso})`)

    if (tournamentsError) throw tournamentsError

    if (!activeTournaments || activeTournaments.length === 0) {
      console.log("No active tournaments to sync.")
      return new Response(
        JSON.stringify({ status: "success", message: "No active tournaments." }),
        { headers: { "Content-Type": "application/json" } }
      )
    }

    // 4. Loop through each active tournament
    for (const tournament of activeTournaments) {
      console.log(`Syncing ESPN data for: ${tournament.name}`)

      // Handle timezone drift by querying yesterday and today
      const today = new Date()
      const yesterday = new Date(today)
      yesterday.setDate(yesterday.getDate() - 1)

      const formatString = (d: Date) => d.toISOString().split('T')[0].replace(/-/g, '')
      const dateRange = `${formatString(yesterday)}-${formatString(today)}`
      
      const scoreboardUrl = new URL(tournament.fixtures_url)
      scoreboardUrl.searchParams.set('dates', dateRange)

      try {
        // Fetch fixtures and standings concurrently
        const [fixturesRes, standingsRes] = await Promise.all([
          fetch(scoreboardUrl.toString()),
          fetch(tournament.pool_standings_url)
        ])

        if (!fixturesRes.ok || !standingsRes.ok) {
            console.error(`ESPN API HTTP error for ${tournament.name}`)
            continue
        }

        const fixturesData = await fixturesRes.json()
        const standingsData = await standingsRes.json()

        // 4a. Map Teams (Extracting from standings guarantees we get all tournament teams)
        const teamsToUpsert = []
        if (standingsData?.children) {
          for (const group of standingsData.children) {
            const entries = group.standings?.entries || []
            for (const entry of entries) {
              const t = entry.team
              if (t) {
                teamsToUpsert.push({
				  tournament_id: tournament.id,
                  team_code: t.abbreviation, 
                  team_name: t.displayName || t.name,
                  pool: poolName,
                  flag_url: t.logos?.[0]?.href || null,
                  world_ranking: null // ESPN standings payload does not include FIFA world rankings
                })
				
			  // ESPN stores stats in an array of objects, we need to extract by name
              const getStat = (statName: string) => entry.stats?.find((s: any) => s.name === statName)?.value

              standingsToUpsert.push({
                tournament_id: tournament.id,
                name: group.name,
                team_code: t.abbreviation,
				played: getStat('gamesPlayed') || 0,
                wins: getStat('wins') || 0,
                draws: getStat('ties') || 0,
                losses: getStat('losses') || 0,
                points: getStat('points') || 0,
                goal_difference: getStat('pointDifferential') || 0,
				goals_for: getStat('pointsFor') || 0,
                goals_against: getStat('pointsAgainst') || 0,
                rank: getStat('rank') || 1
              })
              }
            }
          }
        }
		
        // 4b. Map Fixtures (Scoreboard events)
        const fixturesToUpsert = []
        if (fixturesData?.events) {
          for (const event of fixturesData.events) {
            const comp = event.competitions?.[0]
            if (!comp) continue

            const homeTeam = comp.competitors?.find((c: any) => c.homeAway === 'home')
            const awayTeam = comp.competitors?.find((c: any) => c.homeAway === 'away')

            fixturesToUpsert.push({
              id: event.id,
              tournament_id: tournament.id,
              match_start: event.date,
              stage: event.season?.slug, // e.g., "group-stage", "round-of-16"
			  home_code: homeTeam?.team?.abbreviation || null,
			  away_code: awayTeam?.team?.abbreviation || null,
			  home_score: homeTeam?.score ? parseInt(homeTeam.score) : null,
              away_score: awayTeam?.score ? parseInt(awayTeam.score) : null,
			  home_extra: homeTeam?.shootoutScore ? parseInt(homeTeam.shootoutScore) : null,
              away_extra: awayTeam?.shootoutScore ? parseInt(awayTeam.shootoutScore) : null,
			  is_completed: status.type.completed || false,
			  match_clock: event.status?.displayClock || null
            })
          }
        }

        // 4d. Execute Supabase Upserts
		if (teamsToUpsert.length > 0) {
          const { error: teamsErr } = await supabase
            .from('teams')
            .upsert(teamsToUpsert, { onConflict: 'tournament_id, team_code' })
            
          if (teamsErr) console.error(`Teams upsert error for ${tournament.name}:`, teamsErr)
        }

        if (fixturesToUpsert.length > 0) {
          const { error: fixErr } = await supabase
            .from('fixtures')
            .upsert(fixturesToUpsert, { onConflict: 'id' })
          if (fixErr) console.error(`Fixtures upsert error for ${tournament.name}:`, fixErr)
        }

        if (standingsToUpsert.length > 0) {
          // Note: pool_standings likely needs a composite unique key of (tournament_id, team_id) 
          // in your Supabase table settings for this upsert to work correctly.
          const { error: stdErr } = await supabase
            .from('pool_standings')
            .upsert(standingsToUpsert, { onConflict: 'tournament_id, team_id' })
          if (stdErr) console.error(`Standings upsert error for ${tournament.name}:`, stdErr)
        }

        // 5. Update last_synced_at timestamp on success
        await supabase
          .from('tournaments')
          .update({ last_synced_at: new Date().toISOString() })
          .eq('id', tournament.id)

      } catch (err) {
        // Log the error but continue loop for other tournaments
        console.error(`Failed to sync tournament ${tournament.id}:`, err)
      }
    }

    return new Response(
      JSON.stringify({ status: "success", message: `Synced ${activeTournaments.length} tournament(s).` }),
      { headers: { "Content-Type": "application/json" } }
    )

  } catch (err) {
    console.error("Function error:", err)
    return new Response(String(err?.message ?? err), { status: 500 })
  }
})