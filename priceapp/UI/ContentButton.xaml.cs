using System.Windows.Input;

namespace priceapp.UI
{
    
    public partial class ContentButton
    {
        public ContentButton()
        {
            InitializeComponent();
        }
        
        public event EventHandler Tapped;

        public static readonly BindableProperty CommandProperty = BindableProperty.Create(
            nameof(Command),
            typeof(ICommand),
            typeof(ContentButton)
        );

        public ICommand Command
        {
            get => (ICommand)GetValue(CommandProperty);
            set => SetValue(CommandProperty, value);
        }

        private void TapGestureRecognizer_OnTapped(object sender, EventArgs e)
        {
            if(Tapped != null)
                Tapped(this,new EventArgs());
        }
    }
}