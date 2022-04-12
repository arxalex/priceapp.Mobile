using System;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace priceapp.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SearchPage : ContentPage
    {
        public SearchPage()
        {
            InitializeComponent();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing(); 
            Device.BeginInvokeOnMainThread(async () =>
            {
                await Task.Delay(250); SearchEctry.Focus();
            });
        }

        private void ImageButton_OnClicked(object sender, EventArgs e)
        {
            Navigation.PopAsync();
        }
    }
}