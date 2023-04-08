using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using priceapp.Events.Delegates;
using priceapp.LocalDatabase.Models;

namespace priceapp.LocalDatabase.Repositories.Interfaces;

public interface IShopsLocalRepository
{
    Task<int> InsertAsync(ShopLocalDatabaseModel model);
    Task DeleteAsync(int id);
    Task<List<ShopLocalDatabaseModel>> GetAsync();
    Task<List<ShopLocalDatabaseModel>> GetAsync(Expression<Func<ShopLocalDatabaseModel, bool>> expression);
    Task UpdateAsync(ShopLocalDatabaseModel model);
    event ConnectionErrorHandler BadConnectEvent;
    Task DeleteAllAsync();
}