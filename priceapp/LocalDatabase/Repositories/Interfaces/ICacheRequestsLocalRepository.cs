using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using priceapp.Events.Delegates;
using priceapp.LocalDatabase.Models;

namespace priceapp.LocalDatabase.Repositories.Interfaces;

public interface ICacheRequestsLocalRepository
{
    Task InsertAsync(CacheRequestsLocalDatabaseModel model);
    Task DeleteAsync(int id);
    Task<List<CacheRequestsLocalDatabaseModel>> GetAsync();
    Task<List<CacheRequestsLocalDatabaseModel>> GetAsync(
        Expression<Func<CacheRequestsLocalDatabaseModel, bool>> expression);
    Task<bool> ExistsAsync(string requestName, string requestProperties);
    event ConnectionErrorHandler BadConnectEvent;
    Task DeleteAllAsync();
}