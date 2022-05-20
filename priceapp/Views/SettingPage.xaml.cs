using System;
using priceapp.ViewModels.Interfaces;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace priceapp.Views;

[XamlCompilation(XamlCompilationOptions.Compile)]
public partial class SettingPage
{
    public readonly ISettingsViewModel _settingsViewModel;

    public SettingPage()
    {
        InitializeComponent();

        _settingsViewModel = DependencyService.Get<ISettingsViewModel>(DependencyFetchTarget.NewInstance);

        BindingContext = _settingsViewModel;
    }

    private async void ImageButton_OnClicked(object sender, EventArgs e)
    {
        await Navigation.PopAsync();
    }
}