using priceapp.Annotations;
using SQLite;

namespace priceapp.LocalDatabase.Models;

public class CategoryLocalDatabaseModel
{
    [PrimaryKey, AutoIncrement]
    public int RecordId { get; set; }
    public string Label { get; set; }
    public string Image { get; set; }
    public int Id { get; set; }
    [CanBeNull]
    public int? Parent { get; set; }
    public bool IsFilter { get; set; }
}