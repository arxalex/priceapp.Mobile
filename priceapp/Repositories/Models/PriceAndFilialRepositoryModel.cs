// ReSharper disable InconsistentNaming
namespace priceapp.Repositories.Models;

public class PriceAndFilialRepositoryModel
{
    public int itemId { get; set; }
    public double price { get; set; }
    public double quantity { get; set; }
    public int filialId { get; set; }
    public string filialCity { get; set; }
    public string filialStreet { get; set; }
    public string filialHouse { get; set; }
    public double xCord { get; set; }
    public double yCord { get; set; }
    public int shopId { get; set; }
    public string shopLabel { get; set; }
    public string shopIcon { get; set; }
}