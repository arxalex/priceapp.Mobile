// ReSharper disable InconsistentNaming
namespace priceapp.Repositories.Models;

public class PriceRepositoryModel
{
    public int id { get; set; }
    public int itemId { get; set; }
    public int shopId { get; set; }
    public double price { get; set; }
    public int filialId { get; set; }
    public double quantity { get; set; }
    public double? priceFactor { get; set; }
}