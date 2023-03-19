// ReSharper disable InconsistentNaming
namespace priceapp.Repositories.Models;

public class ShoppingListRepositoryModel
{
    public int itemId { get; set; }
    public int? filialId { get; set; }
    public double count { get; set; }
}