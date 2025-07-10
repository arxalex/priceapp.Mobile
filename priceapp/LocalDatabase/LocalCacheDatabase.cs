using priceapp.LocalDatabase.Models;
using SQLite;

namespace priceapp.LocalDatabase;

public static class LocalCacheDatabase
{
    private static SQLiteAsyncConnection _database;

    public static SQLiteAsyncConnection Database
    {
        get
        {
            if (_database != null) return _database;
            _database = new SQLiteAsyncConnection(Constants.CacheDatabasePath, Constants.Flags);
            Task.Run(async () =>
            {
                await _database.CreateTableAsync<CacheRequestsLocalDatabaseModel>();
                await _database.CreateTableAsync<CategoryLocalDatabaseModel>();
                await _database.CreateTableAsync<FilialLocalDatabaseModel>();
                await _database.CreateTableAsync<ItemLocalDatabaseModel>();
                await _database.CreateTableAsync<ShopLocalDatabaseModel>();
                await _database.CreateTableAsync<BrandAlertLocalDatabaseModel>();
            });

            return _database;
        }
    }
}