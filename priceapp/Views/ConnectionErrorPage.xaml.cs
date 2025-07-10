using priceapp.Events.Models;


namespace priceapp.Views;


public partial class ConnectionErrorPage
{
    public ConnectionErrorPage(ConnectionErrorArgs args)
    {
        InitializeComponent();
        StatusCodeLabel.Text = "StatusCode: " + args.StatusCode;
        ErrorMessage.Text = "Error: " + args.Message;
        if (Application.Current.Windows[0].Page.Navigation.NavigationStack.Count > 0)
        {
            HeaderBackButton.IsVisible = true;
        }
    }

    private void Button_OnClicked(object sender, EventArgs e)
    {
        Application.Current.Windows[0].Page = ((App)Application.Current).OnCreateWindow();
    }

    private void HeaderBackButton_OnBackButtonClicked(object sender, EventArgs e)
    {
        Navigation.PopAsync();
    }
}