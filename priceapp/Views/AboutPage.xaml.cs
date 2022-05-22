using System;
using Xamarin.Essentials;
using Xamarin.Forms.Xaml;

namespace priceapp.Views;

[XamlCompilation(XamlCompilationOptions.Compile)]
public partial class AboutPage
{
    public AboutPage()
    {
        InitializeComponent();
        Label.Text = "Версія: " + VersionTracking.CurrentBuild;
    }

    private async void ImageButton_OnClicked(object sender, EventArgs e)
    {
        await Navigation.PopAsync();
    }
}