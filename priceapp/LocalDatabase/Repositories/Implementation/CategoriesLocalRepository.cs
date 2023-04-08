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

    public async Task<int> InsertAsync(CategoryLocalDatabaseModel model)
    {
        await _connection.InsertAsync(model);
        return model.RecordId;
    }

    public async Task DeleteAsync(int id)
    {
        await _connection.DeleteAsync<CategoryLocalDatabaseModel>(id);
    }

    public async Task<List<CategoryLocalDatabaseModel>> GetAsync()
    {
        return await _connection.Table<CategoryLocalDatabaseModel>().ToListAsync();
    }

    public async Task<List<CategoryLocalDatabaseModel>> GetAsync(
        Expression<Func<CategoryLocalDatabaseModel, bool>> expression)
    {
        return await _connection.Table<CategoryLocalDatabaseModel>().Where(expression).ToListAsync();
    }

    public async Task UpdateAsync(CategoryLocalDatabaseModel model)
    {
        await _connection.UpdateAsync(model);
    }
    
    public async Task DeleteAllAsync()
    {
        await _connection.DeleteAllAsync<CategoryLocalDatabaseModel>();
    }
}