using System.Text.Json.Serialization;
using _ = FamilySweepstake.Models.Constants;

namespace FamilySweepstake.Models;

public record TeamOwnershipModel(
    Guid Id,
    [property: JsonPropertyName("tournament_id")] Guid TournamentId,
    [property: JsonPropertyName("team_code")] string? TeamCode = _.DEFAULT_CODE,
    [property: JsonPropertyName("owner_id")] Guid? OwnerId = null,
    [property: JsonPropertyName("is_playoffs")] bool? IsPlayoffs = false
);
