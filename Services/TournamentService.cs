// Ignore Spelling: eq supabase

using System.Net.Http.Json;
using FamilySweepstake.Models;

namespace FamilySweepstake.Services;

public class SupabaseTournamentService : ITournamentService
{
    private readonly HttpClient _http;
    private readonly PeriodicTimer _heartbeatTimer;
    private readonly CancellationTokenSource _cts;
    private readonly Task _heartbeatTask;
    private readonly TournamentCache _tournamentCache;
    public SupabaseTournamentService(HttpClient http, TournamentCache tournamentCache)
    {
        _http = http;

        // Setup the background cancellation token and 5-minute timer
        _cts = new CancellationTokenSource();
        _heartbeatTimer = new PeriodicTimer(TimeSpan.FromMinutes(5));

        // Fire and forget the background loop the moment the service is instantiated
        _heartbeatTask = RunHeartbeatLoopAsync(_cts.Token);

        _tournamentCache = tournamentCache;
    }

    public async Task<List<FamilyMemberModel>> GetFamilyMembersAsync()
        => await _http.GetFromJsonAsync<List<FamilyMemberModel>>("family_members?select=*") ?? [];

    public async Task<List<TournamentModel>> GetTournamentsAsync()
        => (await _http.GetFromJsonAsync<List<TournamentModel>>($"tournaments?select=*")) ?? [];

    public TournamentModel? GetTournament(string tournamentCode)
        => _tournamentCache.GetByCode(tournamentCode);

    public Task<List<TeamModel>> GetTeamsAsync(string tournamentCode)
        => GetTeamsAsync(_tournamentCache.GetIdByCode(tournamentCode));

    public async Task<List<TeamModel>> GetTeamsAsync(Guid tournamentId)
        => await _http.GetFromJsonAsync<List<TeamModel>>($"teams?tournament_id=eq.{tournamentId}&select=*&order=world_ranking.asc") ?? [];

    public Task<List<FixtureModel>> GetBracketFixturesAsync(string tournamentCode)
        => GetBracketFixturesAsync(_tournamentCache.GetIdByCode(tournamentCode));

    public async Task<List<FixtureModel>> GetBracketFixturesAsync(Guid tournamentId)
        => await _http.GetFromJsonAsync<List<FixtureModel>>($"fixtures?tournament_id=eq.{tournamentId}&bracket_slot=not.is.null&select=*&order=match_start.asc") ?? [];

    public Task<List<FixtureModel>> GetFixturesAsync(string tournamentCode)
        => GetFixturesAsync(_tournamentCache.GetIdByCode(tournamentCode));

    public async Task<List<FixtureModel>> GetFixturesAsync(Guid tournamentId)
        => await _http.GetFromJsonAsync<List<FixtureModel>>($"fixtures?tournament_id=eq.{tournamentId}&select=*&order=match_start.asc") ?? [];

    public Task<List<PoolStandingModel>> GetPoolStandingsAsync(string tournamentCode)
        => GetPoolStandingsAsync(_tournamentCache.GetIdByCode(tournamentCode));

    public async Task<List<PoolStandingModel>> GetPoolStandingsAsync(Guid tournamentId)
        => await _http.GetFromJsonAsync<List<PoolStandingModel>>($"pool_standings?tournament_id=eq.{tournamentId}&select=*&order=pool_ranking.asc") ?? [];

    public Task<List<TeamOwnershipModel>> GetTeamOwnershipsAsync(string tournamentCode)
        => GetTeamOwnershipsAsync(_tournamentCache.GetIdByCode(tournamentCode));

    public async Task<List<TeamOwnershipModel>> GetTeamOwnershipsAsync(Guid tournamentId)
        => await _http.GetFromJsonAsync<List<TeamOwnershipModel>>($"team_ownership?tournament_id=eq.{tournamentId}&select=*") ?? [];

    // Clean up memory and background tasks when the service is destroyed
    protected bool _disposed;

    void IDisposable.Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    protected virtual void Dispose(bool disposing)
    {
        if (_disposed) return;

        if (disposing)
        {
            _cts.Cancel();
            _heartbeatTimer.Dispose();
            _cts.Dispose();
        }

        _disposed = true;
    }

    private async Task RunHeartbeatLoopAsync(CancellationToken token)
    {
        try
        {
            // Fire the very first heartbeat immediately on startup
            await UpdateHeartbeatAsync();

            // Wait for the 5-minute tick, then run again until cancellation is requested
            while (await _heartbeatTimer.WaitForNextTickAsync(token))
            {
                await UpdateHeartbeatAsync();
            }
        }
        catch (OperationCanceledException)
        {
            // The app is shutting down and cancelled the token. Normal behavior.
        }
    }

    private async Task UpdateHeartbeatAsync()
    {
        try
        {
            var payload = new { last_client_heartbeat = DateTime.UtcNow };

            // Using PatchAsJsonAsync to update the single row in the app_state table
            await _http.PatchAsJsonAsync("app_state?id=eq.1", payload);
        }
        catch (Exception)
        {
            // If a heartbeat fails (e.g., user is driving through a tunnel and loses signal),
            // we swallow the error so it doesn't crash the background loop. 
            // It will simply try again in 5 minutes.
        }
    }
}
