
// ReSharper disable InconsistentNaming

namespace priceapp.Repositories.Models;

public class ItemRepositoryModel
{
    public int id { get; set; }
    public string label { get; set; }
    public string image { get; set; }
    public int category { get; set; }
    public string categoryLabel { get; set; }
    public int brand { get; set; }
    public string brandLabel { get; set; }
    public int package { get; set; }
    public string packageLabel { get; set; }
    public string packageUnits { get; set; }
    public double units { get; set; }
    public double term { get; set; }
    public int[] consist { get; set; }
    public double? calorie { get; set; }
    public double? carbohydrates { get; set; }
    public double? fat { get; set; }
    public double? proteins { get; set; }
    public object? additional { get; set; }
    public double priceMin { get; set; }
    public double priceMax { get; set; }
}