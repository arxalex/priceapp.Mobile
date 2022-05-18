using System.Collections.Generic;
using System.Threading.Tasks;
using priceapp.Events.Delegates;
using priceapp.Repositories.Models;

namespace priceapp.Repositories.Interfaces;

public interface IShopRepository
{
    Task<IList<ShopRepositoryModel>> GetShops();
    Task<IList<FilialRepositoryModel>> GetFilials();
    event ConnectionErrorHandler BadConnectEvent;
}