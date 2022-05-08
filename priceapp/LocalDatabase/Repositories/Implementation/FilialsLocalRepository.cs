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

/*[assembly: Dependency(typeof(FilialsLocalRepository))]*/
namespace priceapp.LocalDatabase.Repositories.Implementation;

public class FilialsLocalRepository : IFilialsLocalRepository
{
    public event ConnectionErrorHandler BadConnectEvent;
    
    private readonly SQLiteAsyncConnection _connection;
    public FilialsLocalRepository()
    {
        _connection = LocalCacheDatabase.Database;
    }

    public async Task AddItem(ItemToBuyLocalDatabaseModel model)
    {
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
        await _connection.UpdateAsync(model);
    }
}