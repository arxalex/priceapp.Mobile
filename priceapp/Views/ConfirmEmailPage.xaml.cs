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

    private void ButtonLogin_OnClicked(object sender, EventArgs e)
    {
        Navigation.PushAsync(new LoginPage());
    }
}