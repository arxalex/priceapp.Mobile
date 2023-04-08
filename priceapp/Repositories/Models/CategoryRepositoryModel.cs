// ReSharper disable InconsistentNaming
namespace priceapp.Repositories.Models;

public class CategoryRepositoryModel
{
    public int id { get; set; }
    public string label { get; set; }
    public int? parent { get; set; }
    public bool isFilter { get; set; }
    public string image { get; set; }
}