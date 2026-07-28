using FamilySweepstake.Models;

namespace FamilySweepstake.ViewModels;

public class PoolGroupViewModel(string pool, IEnumerable<PoolStandingModel> teams)
{
    public string Pool { get; } = pool;
    public IEnumerable<PoolStandingModel> Teams { get; } = teams;
}
