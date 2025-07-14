using priceapp.Events.Models;
using priceapp.Services;

namespace priceapp.Views;


public partial class ConnectionErrorPage : ContentPage
{
    public static readonly BindableProperty StatusCodeProperty = 
        BindableProperty.Create(nameof(StatusCode), typeof(int), typeof(ConnectionErrorPage));

    public static readonly BindableProperty ErrorMessageProperty = 
        BindableProperty.Create(nameof(ErrorMessage), typeof(string), typeof(ConnectionErrorPage));

    public int StatusCode
    {
        get => (int)GetValue(StatusCodeProperty);
        set => SetValue(StatusCodeProperty, value);
    }

    public string ErrorMessage
    {
        get => (string)GetValue(ErrorMessageProperty);
        set => SetValue(ErrorMessageProperty, value);
    }

    public ConnectionErrorPage(ConnectionErrorArgs args)
    {
        InitializeComponent();
        
        StatusCode = args.StatusCode;
        ErrorMessage = args.Message;
        
        if (Application.Current.Windows[0].Page.Navigation.NavigationStack.Count > 0)
        {
            HeaderBackButton.IsVisible = true;
        }
    }
    
    public ConnectionErrorPage()
    {
        InitializeComponent();

        HeaderBackButton.IsVisible = false;
    }

    private void Button_OnClicked(object sender, EventArgs e)
    {
        InitService.OnStart(Application.Current!.Handler!.MauiContext!.Services
            .GetRequiredService<IServiceProvider>());
    }

    private void HeaderBackButton_OnBackButtonClicked(object sender, EventArgs e)
    {
        Navigation.PopAsync();
    }
}