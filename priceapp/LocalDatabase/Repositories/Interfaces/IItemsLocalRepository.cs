using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using priceapp.Events.Delegates;
using priceapp.LocalDatabase.Models;

namespace priceapp.LocalDatabase.Repositories.Interfaces;

public interface IItemsLocalRepository
{
    Task<int> AddItem(ItemLocalDatabaseModel model);
    Task RemoveItem(int id);
    Task<List<ItemLocalDatabaseModel>> GetItems();
    Task<List<ItemLocalDatabaseModel>> GetItems(Expression<Func<ItemLocalDatabaseModel, bool>> expression);
    Task UpdateItem(ItemLocalDatabaseModel model);
    event ConnectionErrorHandler BadConnectEvent;
}