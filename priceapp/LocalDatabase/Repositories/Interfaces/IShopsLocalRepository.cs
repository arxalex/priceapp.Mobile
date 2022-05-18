using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using priceapp.Events.Delegates;
using priceapp.LocalDatabase.Models;

namespace priceapp.LocalDatabase.Repositories.Interfaces;

public interface IShopsLocalRepository
{
    Task<int> AddShop(ShopLocalDatabaseModel model);
    Task RemoveShop(int id);
    Task<List<ShopLocalDatabaseModel>> GetShops();
    Task<List<ShopLocalDatabaseModel>> GetShops(Expression<Func<ShopLocalDatabaseModel, bool>> expression);
    Task UpdateShop(ShopLocalDatabaseModel model);
    event ConnectionErrorHandler BadConnectEvent;
}