using System.Linq.Expressions;
using priceapp.Events.Delegates;
using priceapp.LocalDatabase.Models;
using priceapp.LocalDatabase.Repositories.Interfaces;
using SQLite;

namespace priceapp.LocalDatabase.Repositories.Implementation;

public class ItemsLocalRepository : IItemsLocalRepository
{
    public event ConnectionErrorHandler? BadConnectEvent;
    
    private readonly SQLiteAsyncConnection _connection = LocalCacheDatabase.Database;

    public async Task<int> InsertAsync(ItemLocalDatabaseModel model)
    {
        await _connection.InsertAsync(model);
        return model.RecordId;
    }

    public async Task DeleteAsync(int id)
    {
        await _connection.DeleteAsync<ItemLocalDatabaseModel>(id);
    }

    public async Task<List<ItemLocalDatabaseModel>> GetAsync()
    {
        return await _connection.Table<ItemLocalDatabaseModel>().ToListAsync();
    }

    public async Task<List<ItemLocalDatabaseModel>> GetAsync(Expression<Func<ItemLocalDatabaseModel, bool>> expression)
    {
        return await _connection.Table<ItemLocalDatabaseModel>().Where(expression).ToListAsync();
    }

    public async Task UpdateAsync(ItemLocalDatabaseModel model)
    {
        await _connection.UpdateAsync(model);
    }
    
    public async Task DeleteAllAsync()
    {
        await _connection.DeleteAllAsync<ItemLocalDatabaseModel>();
    }
}