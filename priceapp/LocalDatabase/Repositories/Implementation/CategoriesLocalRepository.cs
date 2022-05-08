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

[assembly: Dependency(typeof(CategoriesLocalRepository))]
namespace priceapp.LocalDatabase.Repositories.Implementation;

public class CategoriesLocalRepository : ICategoriesLocalRepository
{
    public event ConnectionErrorHandler BadConnectEvent;

    private readonly SQLiteAsyncConnection _connection;

    public CategoriesLocalRepository()
    {
        _connection = LocalCacheDatabase.Database;
    }

    public async Task<int> AddCategory(CategoryLocalDatabaseModel model)
    {
        await _connection.InsertAsync(model);
        return model.RecordId;
    }

    public async Task RemoveCategory(int id)
    {
        await _connection.DeleteAsync<CategoryLocalDatabaseModel>(id);
    }

    public async Task<List<CategoryLocalDatabaseModel>> GetCategories()
    {
        return await _connection.Table<CategoryLocalDatabaseModel>().ToListAsync();
    }

    public async Task<List<CategoryLocalDatabaseModel>> GetCategories(
        Expression<Func<CategoryLocalDatabaseModel, bool>> expression)
    {
        return await _connection.Table<CategoryLocalDatabaseModel>().Where(expression).ToListAsync();
    }

    public async Task UpdateCategory(ItemToBuyLocalDatabaseModel model)
    {
        await _connection.UpdateAsync(model);
    }
}