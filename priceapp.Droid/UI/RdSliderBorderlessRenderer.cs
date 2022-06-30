using Android.Content;
using priceapp.Droid.UI;
using priceapp.UI;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

[assembly: ExportRenderer(typeof(RdSliderBorderless), typeof(RdSliderBorderlessRenderer))]

namespace priceapp.Droid.UI
{
    public class RdSliderBorderlessRenderer : SliderRenderer
    {
        public RdSliderBorderlessRenderer(Context context) : base(context)
        {
        }

        protected override void OnElementChanged(ElementChangedEventArgs<Slider> e)
        {
            base.OnElementChanged(e);

            //Configure native control (TextBox)
            if (Control != null)
            {
                Control.Background = null;
            }

            // Configure Slider properties
            if (e.NewElement != null)
            {
            }
        }
    }
}