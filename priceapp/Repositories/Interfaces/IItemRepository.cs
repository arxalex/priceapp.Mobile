using System.Collections.Generic;
using System.Threading.Tasks;
using priceapp.Enums;
using priceapp.Events.Delegates;
using priceapp.Repositories.Models;

namespace priceapp.Repositories.Interfaces;

public interface IItemRepository
{
    Task<IList<ItemRepositoryModel>> GetItems(int categoryId, int from, int to, double? xCord = null,
        double? yCord = null, double? radius = null);

    Task<ItemRepositoryModel> GetItem(int itemId, double? xCord = null,
        double? yCord = null, double? radius = null);

    Task<IList<PriceAndFilialRepositoryModel>> GetPricesAndFilials(int itemId, double xCord, double yCord, int radius);
    event ConnectionErrorHandler BadConnectEvent;

    Task<IList<PriceAndFilialRepositoryModel>> GetShoppingList(List<ItemToBuyRepositoryModel> items, double xCord,
        double yCord,
        int radius, CartProcessingType type);
}