namespace priceapp.Models;

public class ItemToBuy
{
    public int RecordId { get; set; }
    public Item? Item { get; set; }
    public Filial Filial { get; set; }
    public double Count { get; set; } 
    public bool Added { get; set; }
    public string CountLabel => Count + " x " + Item.Units + Item.PackageUnits;
}