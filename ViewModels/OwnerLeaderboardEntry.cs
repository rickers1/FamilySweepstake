// Ignore Spelling: Leaderboard

using FamilySweepstake.Models;

namespace FamilySweepstake.ViewModels;

public record OwnerLeaderboardEntry(
    FamilyMemberModel Owner,
    int TotalPoints,
    IReadOnlyList<TeamLeaderboardEntry?>? Teams
);
