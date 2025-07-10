using priceapp.Annotations;
using SQLite;

namespace priceapp.LocalDatabase.Models;

public class ItemToBuyLocalDatabaseModel
{
    [PrimaryKey, AutoIncrement]
    public int RecordId { get; set; }
    public int ItemId { get; set; }
    [CanBeNull]
    public int? FilialId { get; set; }
    public double Count { get; set; } 
    public bool Added { get; set; }
}