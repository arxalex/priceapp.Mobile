using System;
using Xamarin.Forms.Xaml;

namespace priceapp.Views;

[XamlCompilation(XamlCompilationOptions.Compile)]
public partial class ConfirmEmailPage
{
    public ConfirmEmailPage()
    {
        InitializeComponent();
    }

    private async void ButtonLogin_OnClicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new LoginPage());
    }
}