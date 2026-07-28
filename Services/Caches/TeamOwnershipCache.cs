// Ignore Spelling: Initialize

using FamilySweepstake.Extensions;
using FamilySweepstake.Models;

namespace FamilySweepstake.Services;

public class TeamOwnershipCache
{
    private Dictionary<string, TeamOwnershipModel> _teamOwnersByCode = new(StringComparer.OrdinalIgnoreCase);
    private Guid _tournamentId = default!;

    public async Task InitializeAsync(ITournamentService service, Guid tournamentId)
    {
        _tournamentId = tournamentId;
        var teams = await service.GetTeamOwnershipsAsync(tournamentId);

        _teamOwnersByCode = teams.ToDictionary(
            keySelector: t => t.TeamCode.BuildCacheKey(t.IsPlayoffs),
            elementSelector: t => t,
            StringComparer.OrdinalIgnoreCase);
    }

    public void Clear() => _teamOwnersByCode.Clear();

    public Guid? GetOwnerIdByCode(string? code, bool? isPlayoff = false)
        => GetOrDefault(code, isPlayoff ?? false)?.OwnerId;

    public TeamOwnershipModel? GetByCode(string? code)
        => code is null ? null : _teamOwnersByCode.TryGetValue(code, out var team) ? team : null;

    public IReadOnlyDictionary<string, TeamOwnershipModel> All
        => _teamOwnersByCode;

    public IReadOnlyDictionary<string, TeamOwnershipModel> AllForPools
        => All.Where(t => !t.Key.IsPlayoffsKey()).ToDictionary(t => t.Key, t => t.Value, StringComparer.OrdinalIgnoreCase);

    public IReadOnlyDictionary<string, TeamOwnershipModel> AllForPlayoffs
        => All.Where(t => t.Key.IsPlayoffsKey()).ToDictionary(t => t.Key, t => t.Value, StringComparer.OrdinalIgnoreCase);

    public TeamOwnershipModel GetOrDefault(string? code, bool isPlayoff)
        => GetByCode(code.BuildCacheKey(isPlayoff)) ?? new(Guid.Empty, _tournamentId);
}
