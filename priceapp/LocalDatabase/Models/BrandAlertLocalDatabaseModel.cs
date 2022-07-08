using SQLite;

namespace priceapp.LocalDatabase.Models;

public class BrandAlertLocalDatabaseModel
{
    [PrimaryKey, AutoIncrement] public int RecordId { get; set; }

    public int Id { get; set; }
    public int BrandId { get; set; }
    public string Message { get; set; }
    public string Color { get; set; }
}