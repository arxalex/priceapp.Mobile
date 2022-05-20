using System.Collections.Generic;

namespace priceapp.Models;

public class ItemToBuyGroup : List<ItemToBuy>
{
    public ItemToBuyGroup(string name, List<ItemToBuy> itemToBuys) : base(itemToBuys)
    {
        Name = name;
    }

    public string Name { get; private set; }
}