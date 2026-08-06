using System.Collections.Generic;
using FamilySweepstake.Models;

namespace FamilySweepstake.ViewModels;

/// <summary>
/// Represents a grouped stage of tournament fixtures, such as Quarterfinals or Finals.
/// </summary>
/// <param name="roundSize">The size of the round (e.g., 8, 4, 2).</param>
/// <param name="stage">The display name for this tournament stage.</param>
/// <param name="isMedalRound">A value indicating whether this is a medal round (Championship/3rd Place).</param>
/// <param name="fixtures">The collection of fixtures belonging to this round.</param>
public class BracketRoundViewModel(int roundSize, string stage, bool isMedalRound, List<FixtureModel>? fixtures = null)
{
    /// <summary>
    /// Gets or sets the size of the round (e.g., 8, 4, 2).
    /// </summary>
    public int RoundSize { get; set; } = roundSize;

    /// <summary>
    /// Gets or sets the display name for this tournament stage.
    /// </summary>
    public string Stage { get; set; } = stage;

    /// <summary>
    /// Gets or sets a value indicating whether this is a medal round (Championship/3rd Place).
    /// </summary>
    public bool IsMedalRound { get; set; } = isMedalRound;

    /// <summary>
    /// Gets or sets the collection of fixtures belonging to this round.
    /// </summary>
    public List<FixtureModel> Fixtures { get; set; } = fixtures ?? [];
}

