using priceapp.ViewModels.Interfaces;
using priceapp.Views;

namespace priceapp.Controls;

public partial class NotFound
{
    private readonly IServiceProvider _serviceProvider;

    public NotFound()
    {
        InitializeComponent();
        _serviceProvider = Application.Current!.Handler!.MauiContext!.Services
            .GetRequiredService<IServiceProvider>();
    }


    public new static readonly BindableProperty NavigationProperty = BindableProperty.Create("Navigation", typeof(INavigation), typeof(NotFound), defaultValue:null);
    
    public new INavigation Navigation
    {
        get => (INavigation)GetValue(NavigationProperty);
        set => SetValue(NavigationProperty, value);
    }


    private void Button_OnClicked(object sender, EventArgs e)
    {
        Navigation?.PushAsync(new SettingPage(_serviceProvider.GetRequiredService<ISettingsViewModel>()));
    }
}