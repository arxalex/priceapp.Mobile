namespace priceapp.Utils;

public static class ColorUtil
{
    public static Color? BlackOrWhiteFrontColorByBackground(string colorHex)
    {
        return Color.FromHex(colorHex).GetLuminosity() > 0.7 ? Colors.Black : Colors.White;
    }
}