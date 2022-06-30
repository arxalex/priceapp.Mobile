using priceapp.iOS.UI;
using priceapp.UI;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

[assembly: ExportRenderer(typeof(RdSliderBorderless), typeof(RdSliderBorderlessRenderer))]

namespace priceapp.iOS.UI
{
    public class RdSliderBorderlessRenderer : SliderRenderer
    {
        protected override void OnElementChanged(ElementChangedEventArgs<Slider> e)
        {
            base.OnElementChanged(e);

            //Configure Native control (UITextField)
            if (Control != null)
            {
                Control.Layer.BorderWidth = 0;
            }
        }
    }
}