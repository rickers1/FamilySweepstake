// Ignore Spelling: ddd leaderboard

using MudBlazor;

namespace FamilySweepstake.Models;

public class Constants
{
    public const string DEFAULT_CODE = "TBD";
    public const string DEFAULT_NAME = "Unknown";
    public const string MATCH_TIME_FORMAT = "h:mm tt";
    public const string MATCH_DATE_FORMAT = "ddd, d MMM";
    public const string MATCH_DATE_TIME_FORMAT = MATCH_DATE_FORMAT + " " + MATCH_TIME_FORMAT;
    public const string RANKING_ICONS = "🏆,🥈,🥉,😕";
    public const string CONFUSED_EMOJI = "🤔";
    public const string MEDAL_ROUND_PREFIX = "r2s";
    public const string GOLD_MEDAL_ROUND_PREFIX = MEDAL_ROUND_PREFIX + "2";
    public const string BRONZE_MEDAL_ROUND_PREFIX = MEDAL_ROUND_PREFIX + "1";
    public const string MEDAL_ROUND_STAGE_NAME = "Finals";

    public static readonly ColouredIcon BRACKET_COLOURED_ICON = new(Icons.Material.Filled.AccountTree, Color.Success);
    public static readonly ColouredIcon FIXTURES_COLOURED_ICON = new(Icons.Material.Filled.CalendarMonth, Color.Primary);
    public static readonly ColouredIcon LEADERBOARD_COLOURED_ICON = new(Icons.Material.Filled.Leaderboard, Color.Info);
    public static readonly ColouredIcon POOLS_COLOURED_ICON = new(Icons.Material.Filled.ViewModule, Color.Warning);
}
