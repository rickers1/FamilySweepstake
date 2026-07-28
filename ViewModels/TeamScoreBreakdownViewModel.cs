namespace FamilySweepstake.ViewModels;

public record TeamScoreBreakdownViewModel(
    string TeamCode,
    int PointsAwarded,
    Guid? TeamOwnerId
);
