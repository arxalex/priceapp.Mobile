using System;
using System.Collections.ObjectModel;

namespace priceapp.Models;

public class Item
{
    public int Id { get; set; }
    public string Label { get; set; }
    public string Image { get; set; }
    public int Category { get; set; }
    public string CategoryLabel { get; set; }
    public int Brand { get; set; }
    public string BrandLabel { get; set; }
    public int Package { get; set; }
    public string PackageLabel { get; set; }
    public string PackageUnits { get; set; }
    public double Units { get; set; }
    public double Term { get; set; }
    public ObservableCollection<int> Consist { get; set; } = new();
    public double Calorie { get; set; }
    public double Carbohydrates { get; set; }
    public double Fat { get; set; }
    public double Proteins { get; set; }
    public object Additional { get; set; }
    public double PriceMin { get; set; }
    public double PriceMax { get; set; }

    public string PriceText => (Math.Abs(PriceMax - PriceMin) < 0.01 ? PriceMax : PriceMin + " - " + PriceMax) + " грн";
    public string UnitsText => Units + " " + PackageUnits;
}