using priceapp.Events.Delegates;
using priceapp.Repositories.Models;

namespace priceapp.Repositories.Interfaces;

public interface IShopRepository
{
    Task<IList<ShopRepositoryModel>> GetShops();
    Task<IList<FilialRepositoryModel>> GetFilials();
    event ConnectionErrorHandler BadConnectEvent;
    Task<IList<FilialRepositoryModel>> GetFilialsAround(double xCord, double yCord, int radius);
}