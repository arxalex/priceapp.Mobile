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
    }
}