// Ignore Spelling: Initialize

using FamilySweepstake.Models;

namespace FamilySweepstake.Services;

public class TournamentCache : CacheBase<TournamentModel>
{
    private Dictionary<string, Guid> _idByCode = new(StringComparer.OrdinalIgnoreCase);
    public string? CurrentTournamentCode { get; set; }
    public Guid CurrentTournamentId { get; set; } = Guid.Empty;

    public async Task InitializeAsync(ITournamentService service)
    {
        var tournaments = await service.GetTournamentsAsync();
        Load(tournaments, t => t.Id);
        _idByCode = tournaments.ToDictionary(t => t.Code, t => t.Id, StringComparer.OrdinalIgnoreCase);
    }

    public Guid GetIdByCode(string? code)
        => code is null ? Guid.Empty : _idByCode.TryGetValue(code, out var id) ? id : Guid.Empty;

    public TournamentModel? GetByCode(string? code)
        => code is null ? null : _idByCode.TryGetValue(code, out var id) ? Cache[id] : null;

    public TournamentModel? GetCurrent()
        => GetByCode(CurrentTournamentCode);
}
