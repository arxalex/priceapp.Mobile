using SQLite;

namespace priceapp.LocalDatabase.Models;

public class CacheRequestsLocalDatabaseModel
{    
    [PrimaryKey, AutoIncrement]
    public int Id { get; set; }
    public string RequestName { get; set; }
    public string RequestProperties { get; set; }
    public string ResponseItemIds { get; set; }
    public DateTime Expires { get; set; }
}