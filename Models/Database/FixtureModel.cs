using System.Reflection;
using System.Text.Json.Serialization;

namespace FamilySweepstake.Models;

public class FixtureModel
{
    public string Id { get; set; } = default!;
    [property: JsonPropertyName("tournament_id")] public Guid TournamentId { get; set; }
    [property: JsonPropertyName("match_start")] public DateTime MatchStart { get; set; }
    public string? Stage { get; set; }

    [property: JsonPropertyName("away_code")] public string? AwayCode { get; set; }
    [property: JsonPropertyName("away_score")] public int? AwayScore { get; set; }
    [property: JsonPropertyName("away_pens")] public int? AwayPens { get; set; }
    [property: JsonPropertyName("away_win")] public bool AwayWin { get; set; }

    [property: JsonPropertyName("home_code")] public string? HomeCode { get; set; }
    [property: JsonPropertyName("home_score")] public int? HomeScore { get; set; }
    [property: JsonPropertyName("home_pens")] public int? HomePens { get; set; }
    [property: JsonPropertyName("home_win")] public bool HomeWin { get; set; }

    [property: JsonPropertyName("is_playoffs")] public bool IsPlayoffs { get; set; }
    [property: JsonPropertyName("bracket_slot")] public string? BracketSlotViewModel { get; set; }

    [property: JsonPropertyName("is_completed")] public bool IsCompleted { get; set; }
    [property: JsonPropertyName("match_clock")] public string? MatchClock { get; set; }
}
