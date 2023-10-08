using System;
using priceapp.Views;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace priceapp.Controls;

[XamlCompilation(XamlCompilationOptions.Compile)]
public partial class NotFound
{
    public NotFound()
    {
        InitializeComponent();
    }
    
    public new static readonly BindableProperty NavigationProperty = BindableProperty.Create("Navigation", typeof(INavigation), typeof(NotFound), defaultValue:null);
    
    public new INavigation Navigation
    {
        get => (INavigation)GetValue(NavigationProperty);
        set => SetValue(NavigationProperty, value);
    }


    private void Button_OnClicked(object sender, EventArgs e)
    {
        Navigation?.PushAsync(new SettingPage());
    }
}