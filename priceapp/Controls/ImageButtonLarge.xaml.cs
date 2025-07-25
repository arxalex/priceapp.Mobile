using System.Windows.Input;

namespace priceapp.Controls;

public partial class ImageButtonLarge
{
    public static readonly BindableProperty IdProperty = BindableProperty.Create("Id", typeof(int), typeof(ImageButtonLarge));
    
    public int Id
    {
        get => (int)GetValue(IdProperty);
        set => SetValue(IdProperty, value);
    }

    public static readonly BindableProperty PrimaryTextProperty = BindableProperty.Create("PrimaryText", typeof(string), typeof(ImageButtonLarge));

    public string PrimaryText
    {
        get => (string)GetValue(PrimaryTextProperty);
        set => SetValue(PrimaryTextProperty, value);
    }

    public static readonly BindableProperty ImageSourceProperty = BindableProperty.Create("ImageSource", typeof(ImageSource), typeof(ImageButtonLarge));

    public ImageSource ImageSource
    {
        get => (ImageSource)GetValue(ImageSourceProperty);
        set => SetValue(ImageSourceProperty, value);
    }
    
    public static readonly BindableProperty CommandProperty = BindableProperty.Create("Command", typeof(ICommand), typeof(ImageButtonLarge));

    public ICommand Command
    {
        get => (ICommand)GetValue(CommandProperty);
        set => SetValue(CommandProperty, value);
    }
    
    public static readonly BindableProperty CommandParameterProperty = BindableProperty.Create("CommandParameter", typeof(object), typeof(ImageButtonLarge));

    public object CommandParameter
    {
        get => GetValue(CommandParameterProperty);
        set => SetValue(CommandParameterProperty, value);
    }
    
    public ImageButtonLarge()
    {
        InitializeComponent();
    }
}