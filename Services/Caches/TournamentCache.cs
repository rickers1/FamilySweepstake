// Ignore Spelling: Initialize

using FamilySweepstake.Models;

namespace FamilySweepstake.Services;

public class TournamentCache : CacheBase<TournamentModel>
{
    private Dictionary<string, Guid> _idByCode = new(StringComparer.OrdinalIgnoreCase);
    public string? CurrentTournamentCode { get; set; }
    public Guid CurrentTournamentId { get; set; } = Guid.Empty;

    /// <summary>
    /// Initialises the cache with tournaments from the service, setting the current active tournament.
    /// </summary>
    /// <param name="service">The tournament service.</param>
    public async Task InitializeAsync(ISupabaseService service)
    {
        var tournaments = await service.GetTournamentsAsync();
        Load(tournaments, t => t.Id);
        _idByCode = tournaments.ToDictionary(t => t.Code, t => t.Id, StringComparer.OrdinalIgnoreCase);
    }

    /// <summary>
    /// Gets a tournament ID by its unique code.
    /// </summary>
    /// <param name="code">The tournament code.</param>
    /// <returns>The tournament GUID, or Guid.Empty if not found.</returns>
    public Guid GetIdByCode(string? code)
        => code is null ? Guid.Empty : _idByCode.TryGetValue(code, out var id) ? id : Guid.Empty;

    /// <summary>
    /// Gets a tournament model by its unique code.
    /// </summary>
    /// <param name="code">The tournament code.</param>
    /// <returns>The tournament model, or null if not found.</returns>
    public TournamentModel? GetByCode(string? code)
        => code is null ? null : _idByCode.TryGetValue(code, out var id) ? Cache[id] : null;

    /// <summary>
    /// Gets the currently active tournament model.
    /// </summary>
    /// <returns>The current tournament model, or null if none is set.</returns>
    public TournamentModel? GetCurrent()
        => GetByCode(CurrentTournamentCode);

    /// <summary>
    /// Retrieves all cached tournaments.
    /// </summary>
    /// <returns>An enumerable of tournament models, ordered by start date descending.</returns>
    public IEnumerable<TournamentModel> GetAll()
        => _idByCode.Values.Select(id => Cache[id]).OrderByDescending(t => t.StartDate);
}
