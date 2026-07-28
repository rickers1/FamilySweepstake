namespace FamilySweepstake.ViewModels;

public record BracketRound(
    string Stage,
    IReadOnlyList<BracketMatch> Matches
);
