using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace priceapp.UI
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class RdEntryOutlined : ContentView
    {
        public RdEntryOutlined()
        {
            InitializeComponent();
        }

        public event EventHandler<TextChangedEventArgs> TextChanged;

        public static readonly BindableProperty TextProperty =
            BindableProperty.Create(nameof(Text), typeof(string), typeof(RdEntryOutlined), null);

        public static readonly BindableProperty PlaceholderProperty =
            BindableProperty.Create(nameof(Placeholder), typeof(string), typeof(RdEntryOutlined), null);

        public static readonly BindableProperty PlaceholderColorProperty =
            BindableProperty.Create(nameof(PlaceholderColor), typeof(Color), typeof(RdEntryOutlined), Color.DimGray);

        public static readonly BindableProperty BorderColorProperty =
            BindableProperty.Create(nameof(BorderColor), typeof(Color), typeof(RdEntryOutlined), (Color)Application.Current.Resources["ColorPrimary"]);

        public static readonly BindableProperty FontSizeProperty =
            BindableProperty.Create(nameof(FontSize), typeof(int), typeof(RdEntryOutlined), 18);

        public static readonly BindableProperty KeyboardProperty =
            BindableProperty.Create(nameof(Keyboard), typeof(Keyboard), typeof(RdEntryOutlined), Keyboard.Default);

        public static readonly BindableProperty ClearButtonVisibilityProperty =
            BindableProperty.Create(nameof(Keyboard), typeof(ClearButtonVisibility), typeof(RdEntryOutlined),
                ClearButtonVisibility.Never);

        public static readonly BindableProperty IsPasswordProperty =
            BindableProperty.Create("IsPassword", typeof(bool), typeof(Entry), default(bool));

        public static readonly BindableProperty ReturnTypeProperty =
            BindableProperty.Create(nameof(ReturnType), typeof(ReturnType), typeof(Entry), ReturnType.Default);

        public string Text
        {
            get { return (string) GetValue(TextProperty); }
            set { SetValue(TextProperty, value); }
        }

        public string Placeholder
        {
            get { return (string) GetValue(PlaceholderProperty); }
            set { SetValue(PlaceholderProperty, value); }
        }

        public Color PlaceholderColor
        {
            get { return (Color) GetValue(PlaceholderColorProperty); }
            set { SetValue(PlaceholderColorProperty, value); }
        }

        public Color BorderColor
        {
            get { return (Color) GetValue(BorderColorProperty); }
            set { SetValue(BorderColorProperty, value); }
        }

        public int FontSize
        {
            get { return (int) GetValue(FontSizeProperty); }
            set { SetValue(FontSizeProperty, value); }
        }

        public Keyboard Keyboard
        {
            get { return (Keyboard) GetValue(KeyboardProperty); }
            set { SetValue(KeyboardProperty, value); }
        }

        public ClearButtonVisibility ClearButtonVisibility
        {
            get { return (ClearButtonVisibility) GetValue(ClearButtonVisibilityProperty); }
            set { SetValue(ClearButtonVisibilityProperty, value); }
        }

        public bool IsPassword
        {
            get { return (bool) GetValue(IsPasswordProperty); }
            set { SetValue(IsPasswordProperty, value); }
        }

        public ReturnType ReturnType
        {
            get => (ReturnType) GetValue(ReturnTypeProperty);
            set => SetValue(ReturnTypeProperty, value);
        }

        async void TextBox_Focused(object sender, FocusEventArgs e)
        {
            await TranslateLabelToTitle();
        }

        async void TextBox_Unfocused(object sender, FocusEventArgs e)
        {
            await TranslateLabelToPlaceHolder();
        }

        async Task TranslateLabelToTitle()
        {
            if (string.IsNullOrEmpty(this.Text))
            {
                var placeHolder = PlaceHolderLabel;
                var frame = EntryFrame;
                placeHolder.TranslateTo(-placeHolder.Width * 0.125, -(frame.Height / 2), 100U);
                placeHolder.ScaleTo(0.75, 100U);
                placeHolder.TextColor = BorderColor;
            }
        }

        async Task TranslateLabelToPlaceHolder()
        {
            if (string.IsNullOrEmpty(this.Text))
            {
                var placeHolder = this.PlaceHolderLabel;
                placeHolder.TranslateTo(0, 0, 100U);
                placeHolder.ScaleTo(1, 100U);
                placeHolder.TextColor = PlaceholderColor;
            }
        }

        protected virtual void OnTextChanged(Object sender, TextChangedEventArgs e)
        {
            TextChanged?.Invoke(this, e);
        }
    }
}