using System.Collections.Generic;
using AutoMapper;
using priceapp.Models;
using priceapp.Repositories.Interfaces;
using priceapp.Repositories.Models;
using priceapp.ViewModels;
using priceapp.ViewModels.Interfaces;
using Xamarin.Forms;

[assembly: Dependency(typeof(CategoryViewModel))]
namespace priceapp.ViewModels
{
    public class CategoryViewModel : ICategoryViewModel
    {
        private readonly IMapper _mapper;
        private readonly ICategoryRepository _categoryRepository;

        public CategoryViewModel()
        {
            _mapper = DependencyService.Get<IMapper>();
            _categoryRepository = DependencyService.Get<ICategoryRepository>();
        }

        public IList<Category> Categories { get; set; } = new List<Category>();

        public void Load()
        {
            var list = _categoryRepository.GetCategories();
            Categories = _mapper.Map<IList<CategoryRepositoryModel>, IList<Category>>(list);
        }

        public IList<Category> GetCategories()
        {
            return Categories;
        }
        
        
    }
}