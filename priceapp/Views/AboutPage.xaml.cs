using System;
using Xamarin.Forms.Xaml;

namespace priceapp.Views;

[XamlCompilation(XamlCompilationOptions.Compile)]
public partial class AboutPage
{
    public AboutPage()
    {
        InitializeComponent();
    }

    private async void ImageButton_OnClicked(object sender, EventArgs e)
    {
        await Navigation.PopAsync();
    }
}