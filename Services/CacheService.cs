// Ignore Spelling: Initialize

namespace FamilySweepstake.Services;

public class CacheService(ISupabaseService service)
{
    public ISupabaseService Service => service;
    public TournamentCache Tournaments { get; } = new();
    public FamilyMemberCache FamilyMembers { get; } = new();
    public TeamOwnershipCache TeamOwners { get; } = new();
    public TeamCache Teams { get; } = new();

    public async Task InitializeAsync()
    {
        await Tournaments.InitializeAsync(service);
        await FamilyMembers.InitializeAsync(service);
        await SetTournamentAsync();
    }

    public async Task SetTournamentAsync(string? code = null)
    {
        // Find the most recent enabled tournament by start date
        code ??= Tournaments.All.Values
                .Where(t => t.IsEnabled)
                .OrderByDescending(t => t.StartDate)
                .FirstOrDefault()
                ?.Code;

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
