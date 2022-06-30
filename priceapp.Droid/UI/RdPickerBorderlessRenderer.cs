using Android.Content;
using priceapp.Droid.UI;
using priceapp.UI;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

[assembly: ExportRenderer(typeof(RdPickerBorderless), typeof(RdPickerBorderlessRenderer))]

namespace priceapp.Droid.UI
{
    public class RdPickerBorderlessRenderer : PickerRenderer
    {
        public RdPickerBorderlessRenderer(Context context) : base(context)
        {
        }

        protected override void OnElementChanged(ElementChangedEventArgs<Picker> e)
        {
            base.OnElementChanged(e);

            //Configure native control (TextBox)
            if (Control != null)
            {
                Control.Background = null;
            }

            // Configure Picker properties
            if (e.NewElement != null)
            {
            }
        }
    }
}