using System.Text.Json.Serialization;

namespace FamilySweepstake.Models;

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum TournamentType
{
    Soccer,
    Rugby7,
    Rugby15
}
