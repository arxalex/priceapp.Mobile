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
    public event ConnectionErrorHandler BadConnectEvent;
    
    private readonly SQLiteAsyncConnection _connection;
    public ItemsToBuyLocalRepository()
    {
        _connection = LocalDatabase.Database;
    }

    public async Task AddItem(ItemToBuyLocalDatabaseModel model)
    {
        if (await Exists(model.ItemId, model.FilialId))
        {
            var item = (await GetItems(x => x.ItemId == model.ItemId && x.FilialId == model.FilialId)).First();
            item.Count++;
            await UpdateItem(item);
            return;
        }
        await _connection.InsertAsync(model);
    }

    public async Task RemoveItem(int id)
    {
        await _connection.DeleteAsync<ItemToBuyLocalDatabaseModel>(id);
    }

    public async Task<List<ItemToBuyLocalDatabaseModel>> GetItems()
    {
        return await _connection.Table<ItemToBuyLocalDatabaseModel>().ToListAsync();
    }

    public async Task<List<ItemToBuyLocalDatabaseModel>> GetItems(Expression<Func<ItemToBuyLocalDatabaseModel, bool>> expression)
    {
        return await _connection.Table<ItemToBuyLocalDatabaseModel>().Where(expression).ToListAsync();
    }

    public async Task<bool> Exists(int itemId, int? filialId = null)
    {
        return (await GetItems(x => x.ItemId == itemId && x.FilialId == filialId)).Count > 0;
    }

    public async Task UpdateItem(ItemToBuyLocalDatabaseModel model)
    {
        if (model.Count > 0)
        {
            await _connection.UpdateAsync(model);
            return;
        }

        await RemoveItem(model.ItemId);
    }
}