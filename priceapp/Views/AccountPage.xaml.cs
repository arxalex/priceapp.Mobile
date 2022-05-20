using priceapp.Events.Models;
using priceapp.ViewModels.Interfaces;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace priceapp.Views;

[XamlCompilation(XamlCompilationOptions.Compile)]
public partial class AccountPage
{
    public AccountPage()
    {
        InitializeComponent();

        var accountViewModel = DependencyService.Get<IAccountViewModel>(DependencyFetchTarget.NewInstance);

        accountViewModel.Loaded += AccountViewModelOnLoaded;
        accountViewModel.BadConnectEvent += AccountViewModelOnBadConnectEvent;

        ActivityIndicator.IsRunning = true;
        ActivityIndicator.IsVisible = true;

        accountViewModel.LoadAsync();

        BindingContext = accountViewModel;
    }

    private async void AccountViewModelOnBadConnectEvent(object sender, ConnectionErrorArgs args)
    {
        await Navigation.PushAsync(new ConnectionErrorPage(args));
    }

    private void AccountViewModelOnLoaded(object sender, LoadingArgs args)
    {
        ActivityIndicator.IsRunning = false;
        ActivityIndicator.IsVisible = false;
    }
}