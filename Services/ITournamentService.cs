using FamilySweepstake.Models;

namespace FamilySweepstake.Services;

public interface ITournamentService : IDisposable
{
    Task<List<FamilyMemberModel>> GetFamilyMembersAsync();
    Task<List<TournamentModel>> GetTournamentsAsync();
    TournamentModel? GetTournament(string tournamentCode);
    Task<List<TeamModel>> GetTeamsAsync(string tournamentCode);
    Task<List<TeamModel>> GetTeamsAsync(Guid tournamentId);
    Task<List<FixtureModel>> GetFixturesAsync(string tournamentCode);
    Task<List<FixtureModel>> GetFixturesAsync(Guid tournamentId);
    Task<List<PoolStandingModel>> GetPoolStandingsAsync(string tournamentCode);
    Task<List<PoolStandingModel>> GetPoolStandingsAsync(Guid tournamentId);
    Task<List<TeamOwnershipModel>> GetTeamOwnershipsAsync(string tournamentCode);
    Task<List<TeamOwnershipModel>> GetTeamOwnershipsAsync(Guid tournamentId);
}
