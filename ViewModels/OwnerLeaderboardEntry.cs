// Ignore Spelling: Leaderboard

using FamilySweepstake.Models;

namespace FamilySweepstake.ViewModels;

public record OwnerLeaderboardEntry(
    FamilyMemberModel Owner,
    int Ranking,
    int TotalPoints,
    IReadOnlyList<TeamLeaderboardEntry?>? Teams
)
{
    public string OwnerColoursCss =>
        $"background-color:{Owner.BackgroundColor}; color:{Owner.ForegroundColor};";
}
