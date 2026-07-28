using System.Text.Json.Serialization;

namespace FamilySweepstake.Models;

public record AppStateModel(
    int Id,
    [property: JsonPropertyName("last_client_heartbeat")] DateTimeOffset LastClientHeartbeat
);
