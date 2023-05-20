using System.Windows.Input;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace priceapp.Controls;

[XamlCompilation(XamlCompilationOptions.Compile)]
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
    
    public static readonly BindableProperty SecondaryTextProperty = BindableProperty.Create("SecondaryText", typeof(string), typeof(ImageButtonLarge));

    public string SecondaryText
    {
        get => (string)GetValue(SecondaryTextProperty);
        set => SetValue(SecondaryTextProperty, value);
    }
    
    public static readonly BindableProperty AccentTextProperty = BindableProperty.Create("AccentText", typeof(string), typeof(ImageButtonLarge));

    public string AccentText
    {
        get => (string)GetValue(AccentTextProperty);
        set => SetValue(AccentTextProperty, value);
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
        
        GestureRecognizers.Add(new TapGestureRecognizer
        {
            Command = new Command(() =>
            {
                if (Command != null && Command.CanExecute(CommandParameter))
                {
                    Command.Execute(CommandParameter);
                }
            })
        });
    }
}