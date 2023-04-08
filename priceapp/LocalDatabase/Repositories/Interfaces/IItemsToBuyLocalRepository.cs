using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using priceapp.Events.Delegates;
using priceapp.LocalDatabase.Models;

namespace priceapp.LocalDatabase.Repositories.Interfaces;

public interface IItemsToBuyLocalRepository
{
    Task InsertAsync(ItemToBuyLocalDatabaseModel model);
    Task DeleteAsync(int id);
    Task<List<ItemToBuyLocalDatabaseModel>> GetAsync();
    Task<List<ItemToBuyLocalDatabaseModel>> GetAsync(Expression<Func<ItemToBuyLocalDatabaseModel, bool>> expression);
    Task<bool> ExistsAsync(int itemId, int? filialId);
    Task UpdateAsync(ItemToBuyLocalDatabaseModel model);
    event ConnectionErrorHandler BadConnectEvent;
    Task DeleteAllAsync();
}