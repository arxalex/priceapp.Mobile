using Android.Content;
using priceapp.Droid.UI;
using priceapp.UI;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

[assembly:ExportRenderer(typeof(RdEntryBorderless), typeof(RdEntryBorderlessRenderer))]
namespace priceapp.Droid.UI
{
    public class RdEntryBorderlessRenderer : EntryRenderer
    {
        public RdEntryBorderlessRenderer(Context context) : base(context)
        {
        }

        protected override void OnElementChanged(ElementChangedEventArgs<Entry> e)
        {
            base.OnElementChanged(e);

            //Configure native control (TextBox)
            if(Control != null)
            {
                Control.Background = null;
            }

            // Configure Entry properties
            if(e.NewElement != null)
            {

            }
        }
    }
}