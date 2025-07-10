using priceapp.Services.Interfaces;
using priceapp.ViewModels.Interfaces;

namespace priceapp.Views;


public partial class ConfirmEmailPage
{
    private readonly IServiceProvider _serviceProvider;
    public ConfirmEmailPage(IServiceProvider serviceProvider)
    {
        InitializeComponent();
        _serviceProvider = serviceProvider;
    }

    private void ButtonLogin_OnClicked(object sender, EventArgs e)
    {
        Navigation.PushAsync(new LoginPage(_serviceProvider.GetRequiredService<ILoginViewModel>(), _serviceProvider));
    }
}