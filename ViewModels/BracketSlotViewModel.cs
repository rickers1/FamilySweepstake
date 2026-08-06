namespace FamilySweepstake.ViewModels;

/// <summary>
/// Represents a decoded bracket slot containing the round size and slot number.
/// </summary>
/// <param name="roundSize">The size of the round.</param>
/// <param name="slot">The slot number.</param>
public record BracketSlotViewModel(int RoundSize, int Slot)
{
    /// <summary>
    /// Initialises a new instance of the <see cref="BracketSlotViewModel"/> from a parsed tuple.
    /// </summary>
    public BracketSlotViewModel((int RoundSize, int SlotIndex) tuple) : this(tuple.RoundSize, tuple.SlotIndex)
    { }
}
