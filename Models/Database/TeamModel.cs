using System.Text.Json.Serialization;
using _ = FamilySweepstake.Models.Constants;

namespace FamilySweepstake.Models;

public record TeamModel(
    [property: JsonPropertyName("tournament_id")] Guid TournamentId,
    string? Code = _.DEFAULT_CODE,
    string? Name = _.DEFAULT_NAME,
    [property: JsonPropertyName("flag_url")] string? FlagUrl = null,
    [property: JsonPropertyName("world_ranking")] int? WorldRanking = null
);
