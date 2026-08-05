using MudBlazor;

namespace FamilySweepstake.Models;

public class ColouredIcon(string icon, Color? colour = null)
{
    public string Icon { get; set; } = icon;
    public Color Colour { get; set; } = colour ?? Color.Default;
}
