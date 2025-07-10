using priceapp.LocalDatabase.Models;
using SQLite;

namespace priceapp.LocalDatabase;

public static class LocalDatabase
{
    private static SQLiteAsyncConnection _database;
    public static SQLiteAsyncConnection Database
    {
        get
        {
            if (_database == null)
            {
                _database = new SQLiteAsyncConnection(Constants.DatabasePath, Constants.Flags);
                Task.Run(async () =>
                {
                    await _database.CreateTableAsync<ItemToBuyLocalDatabaseModel>();
                });
            }

            return _database;
        }
    }
}