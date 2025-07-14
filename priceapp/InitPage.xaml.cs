using priceapp.Services;

namespace priceapp;

public partial class InitPage
{
    private readonly IServiceProvider _serviceProvider;
    public InitPage(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
        InitializeComponent();
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();
        InitService.OnStart(_serviceProvider);
    }
}