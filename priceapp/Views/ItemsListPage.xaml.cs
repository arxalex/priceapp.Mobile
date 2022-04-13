using System;
using System.Threading.Tasks;
using priceapp.ViewModels.Interfaces;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace priceapp.Views;

[XamlCompilation(XamlCompilationOptions.Compile)]
public partial class ItemsListPage
{
    private readonly IItemsListViewModel _itemsListViewModel;
    private string CategoryName { get; set; }

    public ItemsListPage(int categoryId, string categoryName)
    {
        InitializeComponent();
        CategoryName = categoryName;
        CategoryLabel.Text = CategoryName;

        _itemsListViewModel = DependencyService.Get<IItemsListViewModel>();
        _itemsListViewModel.CategoryId = categoryId;

        BindingContext = _itemsListViewModel;

        _itemsListViewModel.Reload();
        _itemsListViewModel.LoadAsync();
    }

    private void ImageButton_OnClicked(object sender, EventArgs e)
    {
        Navigation.PopAsync();
    }

    private void CollectionView_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        Navigation.PushAsync(new ItemPage());
    }
}