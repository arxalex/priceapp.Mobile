using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using priceapp.Events.Delegates;
using priceapp.LocalDatabase.Models;

namespace priceapp.LocalDatabase.Repositories.Interfaces;

public interface IShopsLocalRepository
{
    Task AddItem(ItemToBuyLocalDatabaseModel model);
    Task RemoveItem(int id);
    Task<List<ItemToBuyLocalDatabaseModel>> GetItems();
    Task<List<ItemToBuyLocalDatabaseModel>> GetItems(Expression<Func<ItemToBuyLocalDatabaseModel, bool>> expression);
    Task<bool> Exists(int itemId, int? filialId);
    Task UpdateItem(ItemToBuyLocalDatabaseModel model);
    event ConnectionErrorHandler BadConnectEvent;
}