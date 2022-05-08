using SQLite;

namespace priceapp.LocalDatabase.Models;

public class ShopLocalDatabaseModel
{
    [PrimaryKey, AutoIncrement]
    public int RecordId { get; set; }
    public int Id { get; set; }
    public string Label { get; set; }
    public string Short { get; set; }
    public string Icon { get; set; }
}