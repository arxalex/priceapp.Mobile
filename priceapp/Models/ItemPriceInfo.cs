using Xamarin.Forms;
using Xamarin.Forms.Maps;
using Xamarin.Forms.Xaml;

namespace priceapp.Models;

public class ItemPriceInfo
{
    public int ItemId { get; set; }
    public Shop Shop { get; set; }
    public Filial Filial { get; set; }
    public double Price { get; set; }
    public double Quantity { get; set; }

    public string PriceView => Price + " грн";
    public string ShopItemPriceInfo => Shop.Label + ": " + Price + " грн";
    public string QuantityView => Quantity > 0 ? "Є в наявності" : "Немає в наявності";
    public Color QuantityViewColor => Quantity > 0 ? (Color)Application.Current.Resources["ColorPrimary"] : Color.Red;

    public Position Position => new(Filial.YCord, Filial.XCord);
}