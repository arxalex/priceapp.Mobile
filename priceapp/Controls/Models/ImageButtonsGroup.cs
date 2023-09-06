using System.Collections.Generic;

namespace priceapp.Controls.Models;

public class ImageButtonsGroup : List<ImageButtonModel>
{
    public ImageButtonsGroup(string name, List<ImageButtonModel> itemToBuys) : base(itemToBuys)
    {
        Name = name;
    }

    public string Name { get; private set; }
}