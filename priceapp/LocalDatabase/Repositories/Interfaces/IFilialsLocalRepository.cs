using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using priceapp.Events.Delegates;
using priceapp.LocalDatabase.Models;

namespace priceapp.LocalDatabase.Repositories.Interfaces;

public interface IFilialsLocalRepository
{
    Task<int> InsertAsync(FilialLocalDatabaseModel model);
    Task DeleteAsync(int id);
    Task<List<FilialLocalDatabaseModel>> GetAsync();
    Task<List<FilialLocalDatabaseModel>> GetAsync(Expression<Func<FilialLocalDatabaseModel, bool>> expression);
    Task UpdateAsync(FilialLocalDatabaseModel model);
    event ConnectionErrorHandler BadConnectEvent;
    Task DeleteAllAsync();
}