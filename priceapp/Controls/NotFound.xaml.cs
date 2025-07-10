using priceapp.ViewModels.Interfaces;
using priceapp.Views;

namespace priceapp.Controls;

public partial class NotFound
{
    private readonly IServiceProvider _serviceProvider;
    public NotFound(IServiceProvider serviceProvider)
    {
        InitializeComponent();
        _serviceProvider = serviceProvider;
    }

    public NotFound()
    {
        throw new NotImplementedException();
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