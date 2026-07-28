using System.Text.Json.Serialization;

namespace FamilySweepstake.ViewModels;

public class TeamViewModel : ITeamBaseViewModel
{
    [JsonPropertyName("tournament_id")] public Guid TournamentId { get; set; }
    public string TeamCode { get; set; } = "TBD";
    public string Name { get; set; } = "Unknown";
    [JsonPropertyName("pool")] public string Pool { get; set; } = "TBD";
    [JsonPropertyName("flag_url")] public string FlagUrl { get; set; } = string.Empty;
    [JsonPropertyName("world_ranking")] public int? WorldRanking { get; set; }
    [JsonPropertyName("advance_to_playoffs")] public bool? AdvanceToPlayoffs { get; set; }
    [JsonPropertyName("owner_id")] public Guid? OwnerId { get; set; }
    [JsonPropertyName("is_playoffs")] public bool? IsPlayoffs { get; set; }
}
