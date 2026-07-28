// Ignore Spelling: Initialize

using FamilySweepstake.Models;

namespace FamilySweepstake.Services;

public class TeamCache
{
    private Dictionary<string, TeamModel> _teamsByCode = new(StringComparer.OrdinalIgnoreCase);
    private Guid _tournamentId = default!;

    public async Task InitializeAsync(ITournamentService service, Guid tournamentId)
    {
        _tournamentId = tournamentId;
        var teams = await service.GetTeamsAsync(tournamentId);

        _teamsByCode = teams.ToDictionary(
            keySelector: t => t.Code!,
            elementSelector: t => t,
            StringComparer.OrdinalIgnoreCase);
    }

    public void Clear() => _teamsByCode.Clear();

    public TeamModel? GetByCode(string? code)
        => code is null ? null : _teamsByCode.TryGetValue(code, out var team) ? team : null;

    public IReadOnlyDictionary<string, TeamModel> All
        => _teamsByCode;

    public TeamModel GetOrDefault(string? code)
        => GetByCode(code) ?? new(_tournamentId);
}
