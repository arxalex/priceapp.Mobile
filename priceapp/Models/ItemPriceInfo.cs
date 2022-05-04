using Xamarin.Forms;
using Xamarin.Forms.Maps;

namespace priceapp.Models;

public class ItemPriceInfo
{
    public int ItemId { get; set; }
    public Shop Shop { get; set; }
    public Filial Filial { get; set; }
    public double Price { get; set; }
    public double Quantity { get; set; }

    public string PriceView => Price + " грн";
    public string QuantityView => Quantity > 0 ? "Є в наявності" : "Немає в наявності";
    public Color QuantityViewColor => Quantity > 0 ? Color.Accent : Color.Red;

    public Position Position => new(Filial.YCord, Filial.XCord);
}