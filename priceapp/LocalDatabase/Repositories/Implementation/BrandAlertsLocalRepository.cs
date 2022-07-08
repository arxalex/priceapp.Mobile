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

[assembly: Dependency(typeof(BrandAlertsLocalRepository))]

namespace priceapp.LocalDatabase.Repositories.Implementation;

public class BrandAlertsLocalRepository : IBrandAlertsLocalRepository
{
    private readonly SQLiteAsyncConnection _connection;

    public BrandAlertsLocalRepository()
    {
        _connection = LocalCacheDatabase.Database;
    }

    public event ConnectionErrorHandler BadConnectEvent;

    public async Task<int> AddItem(BrandAlertLocalDatabaseModel model)
    {
        await _connection.InsertAsync(model);
        return model.RecordId;
    }

    public async Task RemoveItem(int id)
    {
        await _connection.DeleteAsync<BrandAlertLocalDatabaseModel>(id);
    }

    public async Task<List<BrandAlertLocalDatabaseModel>> GetItems()
    {
        return await _connection.Table<BrandAlertLocalDatabaseModel>().ToListAsync();
    }

    public async Task<List<BrandAlertLocalDatabaseModel>> GetItems(
        Expression<Func<BrandAlertLocalDatabaseModel, bool>> expression)
    {
        return await _connection.Table<BrandAlertLocalDatabaseModel>().Where(expression).ToListAsync();
    }

    public async Task UpdateItem(BrandAlertLocalDatabaseModel model)
    {
        await _connection.UpdateAsync(model);
    }
}