using System.IO;

namespace priceapp;

public class Constants
{
    public const int DefaultRadius = 1000;
    public const bool DefaultShowRussiaSupportBrandAlerts = false;
    public const bool DefaultUseCustomLocation = false;
    public const double DefaultX = 50.4387577;
    public const double DefaultY = 30.5229812;
    public static readonly string DefaultCustomLocation = $"{DefaultX};{DefaultY}";
    private const string DatabaseFilename = "PriceAppSQLite.db3";
    private const string CacheDatabaseFilename = "PriceAppSQLiteCache.db3";
    public readonly static string ApiUrl = "https://api.priceapp.co/";

    public const SQLite.SQLiteOpenFlags Flags =
        SQLite.SQLiteOpenFlags.ReadWrite |
        SQLite.SQLiteOpenFlags.Create |
        SQLite.SQLiteOpenFlags.SharedCache;

    public static string DatabasePath => Path.Combine(Xamarin.Essentials.FileSystem.AppDataDirectory, DatabaseFilename);

    public static string CacheDatabasePath =>
        Path.Combine(Xamarin.Essentials.FileSystem.CacheDirectory, CacheDatabaseFilename);
}