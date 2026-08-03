namespace FamilySweepstake.Extensions;


public static class BracketHelperExtensions
{
    // Parses "r32s1" -> (Round: 32, Slot: 1)
    // Custom handling: Maps r2s1 (Bronze) to a separate virtual round if desired, or r2s2 (Gold) to Final
    public static (int RoundSize, int SlotIndex, string CustomRoundName) ParseSlot(this string? bracketSlot)
    {
        if (string.IsNullOrWhiteSpace(bracketSlot))
            return (0, 0, "Unknown");

        // Split "r" and "s" using regex or simple string manipulation
        var parts = bracketSlot.ToLower().Split('s');
        if (parts.Length != 2) return (0, 0, "Unknown");

        int roundSize = int.Parse(parts[0].Replace("r", ""));
        int slotIndex = int.Parse(parts[1]);

        string name = roundSize switch
        {
            32 => "Round of 32",
            16 => "Round of 16",
            8 => "Quarterfinals",
            4 => "Semifinals",
            2 => slotIndex == 1 ? "Bronze Match" : "Gold Final",
            _ => $"Round of {roundSize}"
        };

        return (roundSize, slotIndex, name);
    }
}
