using Xamarin.Forms;

namespace priceapp.Utils;

public static class ColorUtil
{
    public static Color BlackOrWhiteFrontColorByBackground(string colorHex)
    {
        return Color.FromHex(colorHex).Luminosity > 0.7 ? Color.Black : Color.White;
    }
}