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

[assembly: Dependency(typeof(CacheRequestsLocalRepository))]

namespace priceapp.LocalDatabase.Repositories.Implementation;

public class CacheRequestsLocalRepository : ICacheRequestsLocalRepository
{
    public event ConnectionErrorHandler BadConnectEvent;

    private readonly SQLiteAsyncConnection _connection;

    public CacheRequestsLocalRepository()
    {
        _connection = LocalCacheDatabase.Database;
    }

    public async Task InsertAsync(CacheRequestsLocalDatabaseModel model)
    {
        await _connection.InsertAsync(model);
    }

    public async Task DeleteAsync(int id)
    {
        await _connection.DeleteAsync<CacheRequestsLocalDatabaseModel>(id);
    }

    public async Task<List<CacheRequestsLocalDatabaseModel>> GetAsync()
    {
        return await _connection.Table<CacheRequestsLocalDatabaseModel>().ToListAsync();
    }

    public async Task<List<CacheRequestsLocalDatabaseModel>> GetAsync(
        Expression<Func<CacheRequestsLocalDatabaseModel, bool>> expression)
    {
        return await _connection.Table<CacheRequestsLocalDatabaseModel>().Where(expression).ToListAsync();
    }

    public async Task<bool> ExistsAsync(string requestName, string requestProperties)
    {
        return (await GetAsync(x =>
                x.RequestName == requestName && x.RequestProperties == requestProperties && x.Expires > DateTime.Now))
            .Count > 0;
    }
    
    public async Task DeleteAllAsync()
    {
        await _connection.DeleteAllAsync<CacheRequestsLocalDatabaseModel>();
    }
}