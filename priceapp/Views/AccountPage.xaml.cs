using System.Linq;
using priceapp.Events.Models;
using priceapp.ViewModels.Interfaces;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using MenuItem = priceapp.UI.MenuItem;

namespace priceapp.Views;

[XamlCompilation(XamlCompilationOptions.Compile)]
public partial class AccountPage
{
    private readonly IAccountViewModel _accountViewModel = DependencyService.Get<IAccountViewModel>(DependencyFetchTarget.NewInstance);
    public AccountPage()
    {
        InitializeComponent();

        _accountViewModel.Loaded += AccountViewModelOnLoaded;
        _accountViewModel.BadConnectEvent += AccountViewModelOnBadConnectEvent;

        ActivityIndicator.IsRunning = true;
        ActivityIndicator.IsVisible = true;

        _accountViewModel.LoadAsync();

        BindingContext = _accountViewModel;
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

    private async void CollectionView_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        if (CollectionView.SelectedItem == null) return;
        var item = (MenuItem) e.CurrentSelection.FirstOrDefault()!;
        CollectionView.SelectedItem = null;
        switch (item.Label)
        {
            case "Налаштування":
                await Navigation.PushAsync(new SettingPage());
                break;
            case "Про додаток":
                await Navigation.PushAsync(new AboutPage());
                break;
            case "Змінити акаунт":
                await _accountViewModel.ChangeAccount();
                break;
            case "Підказки":
                Application.Current.MainPage = new OnboardingPage();
                break;
            case "Питання та відповіді":
                await Browser.OpenAsync("https://priceapp.co/documents/faq", BrowserLaunchMode.SystemPreferred);
                break;
            case "Політика конфіденційності":
                await Browser.OpenAsync("https://priceapp.co/documents/privacy",
                    BrowserLaunchMode.SystemPreferred);
                break;
            case "Новини":
                await Browser.OpenAsync("https://t.me/price_app", BrowserLaunchMode.SystemPreferred);
                break;
            case "Видалити акаунт":
                await Navigation.PushAsync(new DeleteAccountPage());
                break;
        }

        
    }
}