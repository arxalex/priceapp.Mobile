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

[assembly: Dependency(typeof(FilialsLocalRepository))]
namespace priceapp.LocalDatabase.Repositories.Implementation;

public class FilialsLocalRepository : IFilialsLocalRepository
{
    public event ConnectionErrorHandler BadConnectEvent;
    
    private readonly SQLiteAsyncConnection _connection;
    public FilialsLocalRepository()
    {
        _connection = LocalCacheDatabase.Database;
    }

    public async Task<int> InsertAsync(FilialLocalDatabaseModel model)
    {
        await _connection.InsertAsync(model);
        return model.RecordId;
    }

    public async Task DeleteAsync(int id)
    {
        await _connection.DeleteAsync<FilialLocalDatabaseModel>(id);
    }

    public async Task<List<FilialLocalDatabaseModel>> GetAsync()
    {
        return await _connection.Table<FilialLocalDatabaseModel>().ToListAsync();
    }

    public async Task<List<FilialLocalDatabaseModel>> GetAsync(Expression<Func<FilialLocalDatabaseModel, bool>> expression)
    {
        return await _connection.Table<FilialLocalDatabaseModel>().Where(expression).ToListAsync();
    }

    public async Task UpdateAsync(FilialLocalDatabaseModel model)
    {
        await _connection.UpdateAsync(model);
    }
    
    public async Task DeleteAllAsync()
    {
        await _connection.DeleteAllAsync<FilialLocalDatabaseModel>();
    }
}