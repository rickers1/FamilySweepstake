using FamilySweepstake.Models;

namespace FamilySweepstake.ViewModels;

public record FixtureGroupViewModel(DateTime Date, IEnumerable<FixtureModel> Fixtures, string? Stage);
