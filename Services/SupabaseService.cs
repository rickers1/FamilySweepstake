// Ignore Spelling: eq supabase

using System.Net.Http.Json;
using FamilySweepstake.Models;

namespace FamilySweepstake.Services;

public class SupabaseService : ISupabaseService, IDisposable
{
    private readonly HttpClient _http;
    private readonly PeriodicTimer _heartbeatTimer;
    private readonly CancellationTokenSource _cts;
    private readonly Task _heartbeatTask;
    private readonly IServiceProvider _sp;

    // Lazy-load the cache to completely avoid circular dependency crashes on startup
    private TournamentCache TournamentCache => _sp.GetRequiredService<CacheService>().Tournaments;

    protected bool _disposed;

    /// <summary>
    /// Initialises a new instance of the Supabase data service and starts the heartbeat background task.
    /// </summary>
    /// <param name="http">The configured HTTP client for Supabase.</param>
    /// <param name="sp">The service provider to resolve dependencies lazily.</param>
    public SupabaseService(HttpClient http, IServiceProvider sp)
    {
        _http = http;
        _sp = sp;

        // Setup the background cancellation token and 5-minute timer
        _cts = new CancellationTokenSource();
        _heartbeatTimer = new PeriodicTimer(TimeSpan.FromMinutes(5));

        // Fire and forget the background loop the moment the service is instantiated
        _heartbeatTask = RunHeartbeatLoopAsync(_cts.Token);
    }

    /// <summary>
    /// Retrieves a list of all family members from the database.
    /// </summary>
    /// <returns>A collection of family member models.</returns>
    public async Task<List<FamilyMemberModel>> GetFamilyMembersAsync()
        => await _http.GetFromJsonAsync<List<FamilyMemberModel>>("family_members?select=*") ?? [];

    /// <summary>
    /// Retrieves a list of all available tournaments from the database.
    /// </summary>
    /// <returns>A collection of tournament models.</returns>
    public async Task<List<TournamentModel>> GetTournamentsAsync()
        => await _http.GetFromJsonAsync<List<TournamentModel>>("tournaments?select=*") ?? [];

    /// <summary>
    /// Retrieves a tournament model from the cache using its unique code.
    /// </summary>
    /// <param name="tournamentCode">The tournament string code.</param>
    /// <returns>The tournament model, or null if not found.</returns>
    public TournamentModel? GetTournament(string tournamentCode)
        => TournamentCache.GetByCode(tournamentCode);

    /// <summary>
    /// Retrieves all teams participating in a specific tournament using its string code.
    /// </summary>
    /// <param name="tournamentCode">The tournament string code.</param>
    /// <returns>A collection of team models.</returns>
    public Task<List<TeamModel>> GetTeamsAsync(string tournamentCode)
        => GetTeamsAsync(TournamentCache.GetIdByCode(tournamentCode));

    /// <summary>
    /// Retrieves all teams participating in a specific tournament using its ID.
    /// </summary>
    /// <param name="tournamentId">The unique identifier of the tournament.</param>
    /// <returns>A collection of team models.</returns>
    public async Task<List<TeamModel>> GetTeamsAsync(Guid tournamentId)
        => await _http.GetFromJsonAsync<List<TeamModel>>($"teams?tournament_id=eq.{tournamentId}&select=*&order=world_ranking.asc") ?? [];

    /// <summary>
    /// Retrieves all bracket fixtures for a specific tournament using its string code.
    /// </summary>
    /// <param name="tournamentCode">The tournament string code.</param>
    /// <returns>A collection of fixture models assigned to a bracket slot.</returns>
    public Task<List<FixtureModel>> GetBracketFixturesAsync(string tournamentCode)
        => GetBracketFixturesAsync(TournamentCache.GetIdByCode(tournamentCode));

    /// <summary>
    /// Retrieves all bracket fixtures for a specific tournament using its ID.
    /// </summary>
    /// <param name="tournamentId">The unique identifier of the tournament.</param>
    /// <returns>A collection of fixture models assigned to a bracket slot.</returns>
    public async Task<List<FixtureModel>> GetBracketFixturesAsync(Guid tournamentId)
        => await _http.GetFromJsonAsync<List<FixtureModel>>($"fixtures?tournament_id=eq.{tournamentId}&bracket_slot=not.is.null&select=*") ?? [];

    /// <summary>
    /// Retrieves all fixtures for a specific tournament using its string code.
    /// </summary>
    /// <param name="tournamentCode">The tournament string code.</param>
    /// <returns>A collection of fixture models.</returns>
    public Task<List<FixtureModel>> GetFixturesAsync(string tournamentCode)
        => GetFixturesAsync(TournamentCache.GetIdByCode(tournamentCode));

    /// <summary>
    /// Retrieves all fixtures for a specific tournament using its ID, ordered chronologically.
    /// </summary>
    /// <param name="tournamentId">The unique identifier of the tournament.</param>
    /// <returns>A collection of fixture models.</returns>
    public async Task<List<FixtureModel>> GetFixturesAsync(Guid tournamentId)
        => await _http.GetFromJsonAsync<List<FixtureModel>>($"fixtures?tournament_id=eq.{tournamentId}&select=*&order=match_start.asc") ?? [];

    /// <summary>
    /// Retrieves the pool standings for a specific tournament using its string code.
    /// </summary>
    /// <param name="tournamentCode">The tournament string code.</param>
    /// <returns>A collection of pool standing models.</returns>
    public Task<List<PoolStandingModel>> GetPoolStandingsAsync(string tournamentCode)
        => GetPoolStandingsAsync(TournamentCache.GetIdByCode(tournamentCode));

    /// <summary>
    /// Retrieves the pool standings for a specific tournament using its ID.
    /// </summary>
    /// <param name="tournamentId">The unique identifier of the tournament.</param>
    /// <returns>A collection of pool standing models.</returns>
    public async Task<List<PoolStandingModel>> GetPoolStandingsAsync(Guid tournamentId)
        => await _http.GetFromJsonAsync<List<PoolStandingModel>>($"pool_standings?tournament_id=eq.{tournamentId}&select=*&order=pool_ranking.asc") ?? [];

    /// <summary>
    /// Retrieves the team ownership assignments for a specific tournament using its string code.
    /// </summary>
    /// <param name="tournamentCode">The tournament string code.</param>
    /// <returns>A collection of team ownership models.</returns>
    public Task<List<TeamOwnershipModel>> GetTeamOwnershipsAsync(string tournamentCode)
        => GetTeamOwnershipsAsync(TournamentCache.GetIdByCode(tournamentCode));

    /// <summary>
    /// Retrieves the team ownership assignments for a specific tournament using its ID.
    /// </summary>
    /// <param name="tournamentId">The unique identifier of the tournament.</param>
    /// <returns>A collection of team ownership models.</returns>
    public async Task<List<TeamOwnershipModel>> GetTeamOwnershipsAsync(Guid tournamentId)
        => await _http.GetFromJsonAsync<List<TeamOwnershipModel>>($"team_ownership?tournament_id=eq.{tournamentId}&select=*") ?? [];

    /// <summary>
    /// Disposes of the resources and cancels background tasks.
    /// </summary>
    void IDisposable.Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    /// <summary>
    /// Handles the internal disposal of managed resources.
    /// </summary>
    /// <param name="disposing">True if called from the Dispose method; false if called from a finalizer.</param>
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

    /// <summary>
    /// Background loop that pings the database periodically to keep the application state active.
    /// </summary>
    /// <param name="token">A cancellation token to terminate the loop.</param>
    private async Task RunHeartbeatLoopAsync(CancellationToken token)
    {
        try
        {
            // Fire the very first heartbeat immediately on startup
            await UpdateHeartbeatAsync();

            // Wait for the 5-minute tick, then run again until cancellation is requested
            while (await _heartbeatTimer.WaitForNextTickAsync(token))
                await UpdateHeartbeatAsync();
        }
        catch (OperationCanceledException)
        {
            // The app is shutting down and cancelled the token. Normal behaviour.
        }
    }

    /// <summary>
    /// Sends a heartbeat payload to the Supabase database.
    /// </summary>
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
