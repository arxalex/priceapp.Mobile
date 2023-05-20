using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using priceapp.Controls.Models;
using priceapp.Events.Delegates;
using priceapp.Events.Models;
using priceapp.Models;
using priceapp.Repositories.Interfaces;
using priceapp.ViewModels;
using priceapp.ViewModels.Interfaces;
using priceapp.Views;
using Xamarin.Forms;
using Xamarin.Forms.Internals;

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
        public ObservableCollection<ImageButtonModel> CategoryButtons { get; set; } = new();

        public async Task LoadAsync(INavigation navigation)
        {
            _mapper.Map<List<Category>>(await _categoryRepository.GetCategories()).ForEach(x =>
            {
                Categories.Add(x);
            });
            Categories.Select(x => new ImageButtonModel()
            {
                Id = x.Id,
                Image = x.Image, 
                PrimaryText = x.Label, 
                SecondaryText = null, 
                Command = new Command(async () =>
                {
                    await navigation.PushAsync(new ItemsListPage(x.Id, x.Label));
                })
            }).ForEach(x =>
            {
                CategoryButtons.Add(x);
            });

            Loaded?.Invoke(this,
                new LoadingArgs() { Success = true, LoadedCount = Categories.Count, Total = Categories.Count });
        }
    }
}