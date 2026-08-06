using System.Text.Json.Serialization;

namespace FamilySweepstake.Models;

public record FamilyMemberModel(
    Guid Id,
    string? Name = null,
    [property: JsonPropertyName("avatar_url")] string? AvatarUrl = null,
    [property: JsonPropertyName("foreground_color")] string ForegroundColor = "var(--mud-palette-grey-light)",
    [property: JsonPropertyName("background_color")] string BackgroundColor = "var(--mud-palette-grey-dark)"
);
