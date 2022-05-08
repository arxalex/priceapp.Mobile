namespace priceapp.Models;

public class ItemToBuy
{
    public int Id { get; set; }
    public Item Item { get; set; }
    public Filial Filial { get; set; }
    public int Count { get; set; } 
    public bool Added { get; set; }
}