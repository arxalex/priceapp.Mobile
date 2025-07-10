using System.Collections.ObjectModel;
using AutoMapper;
using priceapp.Controls.Models;
using priceapp.Events.Delegates;
using priceapp.Events.Models;
using priceapp.Models;
using priceapp.Repositories.Interfaces;
using priceapp.Utils;
using priceapp.ViewModels.Interfaces;
using priceapp.Views;

namespace priceapp.ViewModels;

public class CategoryViewModel : ICategoryViewModel
{
    public event ConnectionErrorHandler? BadConnectEvent;
    public event LoadingHandler? Loaded;
    private readonly IMapper _mapper;
    private readonly ICategoryRepository _categoryRepository;
    private readonly IServiceProvider _serviceProvider;

    public CategoryViewModel(
        ICategoryRepository categoryRepository,
        IMapper mapper, 
        IServiceProvider serviceProvider
        ) {
        _categoryRepository = categoryRepository;
        _mapper = mapper;
        _serviceProvider = serviceProvider;
        _categoryRepository.BadConnectEvent += CategoryRepositoryOnBadConnectEvent;
    }

    private void CategoryRepositoryOnBadConnectEvent(object sender, ConnectionErrorArgs args)
    {
        BadConnectEvent?.Invoke(this, args);
    }

    public ObservableCollection<ImageButtonModel> CategoryButtons { get; set; } = new();

    public async Task LoadAsync(INavigation navigation)
    {
        _mapper.Map<List<Category>>(await _categoryRepository.GetCategories())
            .Select(x => new ImageButtonModel
            {
                Id = x.Id,
                Image = x.Image,
                PrimaryText = x.Label,
                Command = new Command( () => { navigation.PushAsync(new ItemsListPage(x.Id, x.Label, _serviceProvider.GetRequiredService<IItemsListViewModel>())); })
            }).ForEach(x => { CategoryButtons.Add(x); });

        Loaded?.Invoke(this,
            new LoadingArgs
                { Success = true, LoadedCount = CategoryButtons.Count, Total = CategoryButtons.Count });
    }
}