namespace FamilySweepstake.Extensions;

public static class FamilySweepstakeExtensions
{
    private const string PLAYOFFS_KEY_SUFFIX = "-Playoff";

    public static string BuildCacheKey(this string? teamCode, bool? isPlayoffs = false)
        => isPlayoffs is true ? $"{teamCode}{PLAYOFFS_KEY_SUFFIX}" : $"{teamCode}";

    public static bool IsPlayoffsKey(this string? key)
        => key?.Contains(PLAYOFFS_KEY_SUFFIX)?? false;
}
