using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using priceapp.Events.Delegates;
using priceapp.LocalDatabase.Models;

namespace priceapp.LocalDatabase.Repositories.Interfaces;

public interface ICacheRequestsLocalRepository
{
    Task AddCacheRecord(CacheRequestsLocalDatabaseModel model);
    Task RemoveCacheRecord(int id);
    Task<List<CacheRequestsLocalDatabaseModel>> GetCacheRecords();
    Task<List<CacheRequestsLocalDatabaseModel>> GetCacheRecords(
        Expression<Func<CacheRequestsLocalDatabaseModel, bool>> expression);
    Task<bool> Exists(string requestName, string requestProperties);
    event ConnectionErrorHandler BadConnectEvent;
}