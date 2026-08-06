using System.Text.Json.Serialization;

namespace FamilySweepstake.Models;

public record PoolStandingModel(
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
    [property: JsonPropertyName("advance_to_playoffs")] bool AdvanceToPlayoffs
);
