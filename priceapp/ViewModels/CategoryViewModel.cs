using System.Collections.ObjectModel;
using System.Threading.Tasks;
using AutoMapper;
using priceapp.Events.Delegates;
using priceapp.Events.Models;
using priceapp.Models;
using priceapp.Repositories.Interfaces;
using priceapp.ViewModels;
using priceapp.ViewModels.Interfaces;
using Xamarin.Forms;

[assembly: Dependency(typeof(CategoryViewModel))]

namespace priceapp.ViewModels
{
    public class CategoryViewModel : ICategoryViewModel
    {
        public event ConnectionErrorHandler BadConnectEvent;
        public event LoadingHandler Loaded;
        private readonly IMapper _mapper;
        private readonly ICategoryRepository _categoryRepository;

        public CategoryViewModel()
        {
            _mapper = DependencyService.Get<IMapper>();
            _categoryRepository = DependencyService.Get<ICategoryRepository>();
            
            _categoryRepository.BadConnectEvent += CategoryRepositoryOnBadConnectEvent;
        }

        private void CategoryRepositoryOnBadConnectEvent(object sender, ConnectionErrorArgs args)
        {
            BadConnectEvent?.Invoke(this, args);
        }

        public ObservableCollection<Category> Categories { get; set; } = new();

        public async Task LoadAsync()
        {
            var categories = await _categoryRepository.GetCategories();

            foreach (var category in categories)
            {
                Categories.Add(_mapper.Map<Category>(category));
            }

            Loaded?.Invoke(this,
                new LoadingArgs() {Success = true, LoadedCount = categories.Count, Total = Categories.Count});
        }
    }
}