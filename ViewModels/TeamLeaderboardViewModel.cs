// Ignore Spelling: Leaderboard

using FamilySweepstake.Models;

namespace FamilySweepstake.ViewModels;

public record TeamLeaderboardViewModel(
    string TeamCode,
    PoolStandingModel Standing,
    int PointsAwarded,
    IReadOnlyList<FixtureModel> Fixtures
);
