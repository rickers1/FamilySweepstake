// Ignore Spelling: Leaderboard

using FamilySweepstake.Models;

namespace FamilySweepstake.ViewModels;

public record LeaderboardViewModel(
    FamilyMemberModel Owner,
    int Ranking,
    int TotalPoints,
    IReadOnlyList<TeamLeaderboardViewModel?>? Teams
)
{
    public string OwnerColoursCss =>
        $"background-color:{Owner.BackgroundColor}; color:{Owner.ForegroundColor};";
}
