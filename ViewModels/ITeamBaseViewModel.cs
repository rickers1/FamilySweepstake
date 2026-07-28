namespace FamilySweepstake.ViewModels;

public interface ITeamBaseViewModel
{
    Guid TournamentId { get; }
    Guid? OwnerId { get; }
    string TeamCode { get; }
}
