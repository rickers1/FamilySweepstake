using System.Text.Json.Serialization;
using FamilySweepstake.Models;

namespace FamilySweepstake.ViewModels;

public record PoolStandingViewModel(
    [property: JsonPropertyName("tournament_id")] Guid TournamentId,
    [property: JsonPropertyName("pool_name")] string PoolName,
    [property: JsonPropertyName("team_code")] string TeamCode,
    int Played,
    int Wins,
    int Draws,
    int Losses,
    int Points,
    [property: JsonPropertyName("pool_ranking")] short PoolRanking,
    [property: JsonPropertyName("rwc_bonus_points")] short RwcBonusPoints,
    [property: JsonPropertyName("fifa_point_deductions")] short FifaPointDeductions,
    [property: JsonPropertyName("advance_to_playoffs")] bool AdvanceToPlayoffs,
    [property: JsonPropertyName("owner_id")] Guid? OwnerId
) : ITeamBaseViewModel
{
    public PoolStandingViewModel(PoolStandingModel pool, TeamOwnershipModel? owner)
        : this(
            pool.TournamentId,
            pool.PoolName,
            pool.TeamCode,
            pool.Played,
            pool.Wins,
            pool.Draws,
            pool.Losses,
            pool.Points,
            pool.PoolRanking,
            pool.RwcBonusPoints,
            pool.FifaPointDeductions,
            pool.AdvanceToPlayoffs,
            owner?.OwnerId
        )
    { }
}
