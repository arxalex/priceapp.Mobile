namespace priceapp.Controls;

public partial class HeaderBackButton
{
    public static readonly BindableProperty LabelProperty = BindableProperty.Create("Label", typeof(string), typeof(HeaderBackButton));

    public string? Label
    {
        get => (string)GetValue(LabelProperty);
        set => SetValue(LabelProperty, value);
    }

    public event EventHandler BackButtonClicked;
    
    public HeaderBackButton()
    {
        InitializeComponent();
    }

    private void ImageButton_OnClicked(object sender, EventArgs e)
    {
        BackButtonClicked?.Invoke(this, e);
    }
}