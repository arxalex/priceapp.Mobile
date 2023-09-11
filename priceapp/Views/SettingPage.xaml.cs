using System;
using priceapp.ViewModels.Interfaces;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace priceapp.Views;

[XamlCompilation(XamlCompilationOptions.Compile)]
public partial class SettingPage
{
    public SettingPage()
    {
        InitializeComponent();

        var settingsViewModel = DependencyService.Get<ISettingsViewModel>(DependencyFetchTarget.NewInstance);

        BindingContext = settingsViewModel;
    }

    private async void ImageButton_OnClicked(object sender, EventArgs e)
    {
        await Navigation.PopAsync();
    }
}