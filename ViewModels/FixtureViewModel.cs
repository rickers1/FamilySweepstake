using System.Text.Json.Serialization;

namespace FamilySweepstake.ViewModels;

public class FixtureViewModel
{
    [JsonPropertyName("id")] public string Id { get; set; } = string.Empty;
    [JsonPropertyName("tournament_id")] public Guid TournamentId { get; set; }
    [JsonPropertyName("match_start")] public DateTime MatchStart { get; set; }
    [JsonPropertyName("stage")] public string? Stage { get; set; }
    [JsonPropertyName("match_clock")] public string? MatchClock { get; set; }
    [JsonPropertyName("is_completed")] public bool IsCompleted { get; set; }
    [JsonPropertyName("is_playoff")] public bool IsPlayoff { get; set; }

    // HOME 
    [JsonPropertyName("home_code")] public string HomeCode { get; set; } = string.Empty;
    [JsonPropertyName("home_score")] public int? HomeScore { get; set; }
    [JsonPropertyName("home_extra")] public int? HomeExtra { get; set; }
    [JsonPropertyName("home_team_name")] public string HomeTeamName { get; set; } = string.Empty;
    [JsonPropertyName("home_flag")] public string HomeFlag { get; set; } = string.Empty;
    [JsonPropertyName("home_world_ranking")] public int? HomeWorldRanking { get; set; }
    [JsonPropertyName("home_owner_id")] public Guid? HomeOwnerId { get; set; }

    // AWAY
    [JsonPropertyName("away_code")] public string AwayCode { get; set; } = string.Empty;
    [JsonPropertyName("away_score")] public int? AwayScore { get; set; }
    [JsonPropertyName("away_extra")] public int? AwayExtra { get; set; }
    [JsonPropertyName("away_team_name")] public string AwayTeamName { get; set; } = string.Empty;
    [JsonPropertyName("away_flag")] public string AwayFlag { get; set; } = string.Empty;
    [JsonPropertyName("away_world_ranking")] public int? AwayWorldRanking { get; set; }
    [JsonPropertyName("away_owner_id")] public Guid? AwayOwnerId { get; set; }
}
