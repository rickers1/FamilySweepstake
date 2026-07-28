using System.Text.Json.Serialization;

namespace FamilySweepstake.Models;

public record TournamentModel(
    Guid Id,
    string Code,
    string Name,
    [property: JsonPropertyName("start_date")] DateTime StartDate,
    [property: JsonPropertyName("end_date")] DateTime EndDate,
    [property: JsonPropertyName("tournament_type")] TournamentType TournamentType,
    [property: JsonPropertyName("is_enabled")] bool IsEnabled,
    [property: JsonPropertyName("fixtures_url")] string FixturesUrl,
    [property: JsonPropertyName("pool_standings_url")] string PoolStandingsUrl,
    [property: JsonPropertyName("rankings_url")] string? RankingsUrl,
    [property: JsonPropertyName("stage_config_url")] string? StageConfigUrl,
    [property: JsonPropertyName("number_of_playoffs_auto_qualifiers")] int NumberOfPlayoffAutoQualifiers,
    [property: JsonPropertyName("last_synced_at")] DateTimeOffset? LastSyncedAt
);
