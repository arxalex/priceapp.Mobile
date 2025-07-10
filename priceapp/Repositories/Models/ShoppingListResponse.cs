namespace priceapp.Repositories.Models;

public class ShoppingListResponse
{
    public List<PriceRepositoryModel> shoppingList { get; set; } = new();
    public double economy { get; set; }
    public List<int> itemIdsNotFound { get; set; } = new();
}