using System.Collections.Generic;
using priceapp.Models;

namespace priceapp.ViewModels.Interfaces
{
    public interface ICategoryViewModel
    {
        IList<Category> Categories { get; set; }
        void Load();
        IList<Category> GetCategories();
    }
}