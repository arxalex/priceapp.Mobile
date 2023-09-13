using System;
using priceapp.Events.Models;
using Xamarin.Forms.Xaml;

namespace priceapp.Views;

[XamlCompilation(XamlCompilationOptions.Compile)]
public partial class ConnectionErrorPage
{
    public ConnectionErrorPage(ConnectionErrorArgs args)
    {
        InitializeComponent();
        StatusCodeLabel.Text = "StatusCode: " + args.StatusCode;
        ErrorMessage.Text = "Error: " + args.Message;
        if (App.Current.MainPage.Navigation.NavigationStack.Count > 0)
        {
            HeaderBackButton.IsVisible = true;
        }
    }

    private void Button_OnClicked(object sender, EventArgs e)
    {
        App.Current.SendStart();
    }

    private void HeaderBackButton_OnBackButtonClicked(object sender, EventArgs e)
    {
        Navigation.PopAsync();
    }
}