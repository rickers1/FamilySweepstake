using FamilySweepstake.Models;

namespace FamilySweepstake.ViewModels;

public record BracketMatch(
    string Stage,
    FixtureModel Fixture,
    string HomeTeam,
    string AwayTeam,
    bool HomeWin,
    bool AwayWin
);
