namespace priceapp;

public class Constants
{
    public const int DefaultRadius = 1000;
    public const bool DefaultShowRussiaSupportBrandAlerts = false;
    public const bool DefaultUseCustomLocation = false;
    public const double DefaultX = 30.5229812;
    public const double DefaultY = 50.4387577;
    public static readonly string DefaultCustomLocation = $"{DefaultX};{DefaultY}";
    private const string DatabaseFilename = "PriceAppSQLite.db3";
    private const string CacheDatabaseFilename = "PriceAppSQLiteCache.db3";
    public readonly static string ApiUrl = "https://api.priceapp.co/";

    public const SQLite.SQLiteOpenFlags Flags =
        SQLite.SQLiteOpenFlags.ReadWrite |
        SQLite.SQLiteOpenFlags.Create |
        SQLite.SQLiteOpenFlags.SharedCache;

    public static string DatabasePath => Path.Combine(FileSystem.AppDataDirectory, DatabaseFilename);

    public static string CacheDatabasePath =>
        Path.Combine(FileSystem.CacheDirectory, CacheDatabaseFilename);
}