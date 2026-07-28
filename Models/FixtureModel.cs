using System.Reflection;
using System.Text.Json.Serialization;

namespace FamilySweepstake.Models;

public record FixtureModel(
    string Id,
    [property: JsonPropertyName("tournament_id")] Guid TournamentId,
    [property: JsonPropertyName("match_start")] DateTime MatchStart,
    string? Stage,

    [property: JsonPropertyName("away_code")] string? AwayCode,
    [property: JsonPropertyName("away_score")] int? AwayScore,
    [property: JsonPropertyName("away_pens")] int? AwayPens,
    [property: JsonPropertyName("away_win")] bool AwayWin,

    [property: JsonPropertyName("home_code")] string? HomeCode,
    [property: JsonPropertyName("home_score")] int? HomeScore,
    [property: JsonPropertyName("home_pens")] int? HomePens,
    [property: JsonPropertyName("home_win")] bool HomeWin,

    [property: JsonPropertyName("is_playoffs")] bool IsPlayoffs,
    [property: JsonPropertyName("is_completed")] bool IsCompleted,
    [property: JsonPropertyName("match_clock")] string? MatchClock
);
