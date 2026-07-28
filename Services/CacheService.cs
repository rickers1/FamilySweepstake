// Ignore Spelling: Initialize

namespace FamilySweepstake.Services;

public class CacheService(ITournamentService service)
{
    public ITournamentService Service => service;
    public TournamentCache Tournaments { get; } = new();
    public FamilyMemberCache FamilyMembers { get; } = new();
    public TeamOwnershipCache TeamOwners { get; } = new();
    public TeamCache Teams { get; } = new();

    public async Task InitializeAsync()
    {
        await Tournaments.InitializeAsync(service);
        await FamilyMembers.InitializeAsync(service);
    }

    public async Task SetTournamentAsync(string? code)
    {
        if (code is null) return;

        if (!string.Equals(Tournaments.CurrentTournamentCode, code, StringComparison.OrdinalIgnoreCase))
        {
            Tournaments.CurrentTournamentCode = code;
            await ReloadTeamsAsync(code);
        }
    }

    private async Task ReloadTeamsAsync(string code)
    {
        var tournamentId = Tournaments.GetIdByCode(code);
        Tournaments.CurrentTournamentId = tournamentId;

        if (tournamentId == Guid.Empty)
        {
            TeamOwners.Clear();
            Teams.Clear();
            return;
        }

        await Teams.InitializeAsync(service, tournamentId);
        await TeamOwners.InitializeAsync(service, tournamentId);
    }
}
