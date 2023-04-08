using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using priceapp.Events.Delegates;
using priceapp.LocalDatabase.Models;
using priceapp.LocalDatabase.Repositories.Implementation;
using priceapp.LocalDatabase.Repositories.Interfaces;
using SQLite;
using Xamarin.Forms;

[assembly: Dependency(typeof(ItemsToBuyLocalRepository))]

namespace priceapp.LocalDatabase.Repositories.Implementation;

public class ItemsToBuyLocalRepository : IItemsToBuyLocalRepository
{
    private readonly SQLiteAsyncConnection _connection;

    public ItemsToBuyLocalRepository()
    {
        _connection = LocalDatabase.Database;
    }

    public event ConnectionErrorHandler BadConnectEvent;

    public async Task InsertAsync(ItemToBuyLocalDatabaseModel model)
    {
        if (await ExistsAsync(model.ItemId, model.FilialId))
        {
            var item = (await GetAsync(x => x.ItemId == model.ItemId && x.FilialId == model.FilialId)).First();
            item.Count++;
            await UpdateAsync(item);
            return;
        }

        await _connection.InsertAsync(model);
    }

    public async Task DeleteAsync(int id)
    {
        await _connection.DeleteAsync<ItemToBuyLocalDatabaseModel>(id);
    }

    public async Task DeleteAllAsync()
    {
        await _connection.DeleteAllAsync<ItemToBuyLocalDatabaseModel>();
    }

    public async Task<List<ItemToBuyLocalDatabaseModel>> GetAsync()
    {
        return await _connection.Table<ItemToBuyLocalDatabaseModel>().ToListAsync();
    }

    public async Task<List<ItemToBuyLocalDatabaseModel>> GetAsync(
        Expression<Func<ItemToBuyLocalDatabaseModel, bool>> expression)
    {
        return await _connection.Table<ItemToBuyLocalDatabaseModel>().Where(expression).ToListAsync();
    }

    public async Task<bool> ExistsAsync(int itemId, int? filialId = null)
    {
        return (await GetAsync(x => x.ItemId == itemId && x.FilialId == filialId)).Count > 0;
    }

    public async Task UpdateAsync(ItemToBuyLocalDatabaseModel model)
    {
        if (model.Count > 0)
        {
            await _connection.UpdateAsync(model);
            return;
        }

        await DeleteAsync(model.ItemId);
    }
}