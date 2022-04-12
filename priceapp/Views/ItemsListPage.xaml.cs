using System;
using priceapp.ViewModels.Interfaces;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace priceapp.Views;

[XamlCompilation(XamlCompilationOptions.Compile)]
public partial class ItemsListPage
{
    private readonly IItemsListViewModel _itemsListViewModel;
    public string CategoryName { get; set; }
    public ItemsListPage(int categoryId, string categoryName)
    {
        InitializeComponent();
        _itemsListViewModel = DependencyService.Get<IItemsListViewModel>();
        _itemsListViewModel.CategoryId = categoryId;

        CategoryName = categoryName;
        
        _itemsListViewModel.Load(0);
        
        CategoryLabel.Text = CategoryName;

        BindingContext = _itemsListViewModel;
    }

    private void ImageButton_OnClicked(object sender, EventArgs e)
    {
        Navigation.PopAsync();
    }

    private void CollectionView_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        throw new NotImplementedException();
    }
}