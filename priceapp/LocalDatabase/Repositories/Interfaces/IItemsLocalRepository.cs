using System.Linq.Expressions;
using priceapp.Events.Delegates;
using priceapp.LocalDatabase.Models;

namespace priceapp.LocalDatabase.Repositories.Interfaces;

public interface IItemsLocalRepository
{
    Task<int> InsertAsync(ItemLocalDatabaseModel model);
    Task DeleteAsync(int id);
    Task<List<ItemLocalDatabaseModel>> GetAsync();
    Task<List<ItemLocalDatabaseModel>> GetAsync(Expression<Func<ItemLocalDatabaseModel, bool>> expression);
    Task UpdateAsync(ItemLocalDatabaseModel model);
    event ConnectionErrorHandler BadConnectEvent;
    Task DeleteAllAsync();
}