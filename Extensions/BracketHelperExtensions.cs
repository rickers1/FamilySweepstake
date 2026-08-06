namespace FamilySweepstake.Extensions;

/// <summary>
/// Provides extension methods for working with bracket structures and string representations.
/// </summary>
public static class BracketHelperExtensions
{
    /// <summary>
    /// Parses a bracket slot string (e.g., "r32s1") into its round size and slot index components.
    /// </summary>
    /// <param name="bracketSlot">The string representation of the bracket slot to parse.</param>
    /// <returns>A tuple containing the round size and slot index. Returns (0, 0) if the input is null or invalid.</returns>
    public static (int RoundSize, int SlotIndex) ParseSlot(this string? bracketSlot)
    {
        if (string.IsNullOrWhiteSpace(bracketSlot))
            return (0, 0);

        // Split "r" and "s" using simple string manipulation
        var parts = bracketSlot.ToLower().Split('s');

        if (parts.Length != 2)
            return (0, 0);

        int roundSize = int.Parse(parts[0].Replace("r", ""));
        int slotIndex = int.Parse(parts[1]);

        return (roundSize, slotIndex);
    }
}
