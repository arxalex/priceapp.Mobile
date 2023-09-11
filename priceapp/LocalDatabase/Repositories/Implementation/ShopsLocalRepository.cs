using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using priceapp.Events.Delegates;
using priceapp.LocalDatabase.Models;
using priceapp.LocalDatabase.Repositories.Implementation;
using priceapp.LocalDatabase.Repositories.Interfaces;
using SQLite;
using Xamarin.Forms;

[assembly: Dependency(typeof(ShopsLocalRepository))]
namespace priceapp.LocalDatabase.Repositories.Implementation;

public class ShopsLocalRepository : IShopsLocalRepository
{
    public event ConnectionErrorHandler BadConnectEvent;
    
    private readonly SQLiteAsyncConnection _connection = LocalCacheDatabase.Database;

    public async Task<int> InsertAsync(ShopLocalDatabaseModel model)
    {
        await _connection.InsertAsync(model);
        return model.RecordId;
    }

    public async Task DeleteAsync(int id)
    {
        await _connection.DeleteAsync<ShopLocalDatabaseModel>(id);
    }

    public async Task<List<ShopLocalDatabaseModel>> GetAsync()
    {
        return await _connection.Table<ShopLocalDatabaseModel>().ToListAsync();
    }

    public async Task<List<ShopLocalDatabaseModel>> GetAsync(Expression<Func<ShopLocalDatabaseModel, bool>> expression)
    {
        return await _connection.Table<ShopLocalDatabaseModel>().Where(expression).ToListAsync();
    }
    
    public async Task UpdateAsync(ShopLocalDatabaseModel model)
    {
        await _connection.UpdateAsync(model);
    }
    
    public async Task DeleteAllAsync()
    {
        await _connection.DeleteAllAsync<ShopLocalDatabaseModel>();
    }
}