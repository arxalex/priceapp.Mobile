using Android.App;
using Android.Content.PM;
using Android.OS;
using Android.Views;
using Xamarin.Forms;
using View = Xamarin.Forms.View;

namespace priceapp.Droid
{
    [Activity(Label = "priceapp",
        Icon = "@mipmap/ic_launcher",
        Theme = "@style/MainTheme",
        MainLauncher = false,
        ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation,
        ScreenOrientation = ScreenOrientation.Portrait)]
    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            TabLayoutResource = Resource.Layout.Tabbar;
            ToolbarResource = Resource.Layout.Toolbar;

            base.OnCreate(savedInstanceState);
            Forms.Init(this, savedInstanceState);
            FormsMaterial.Init(this, savedInstanceState);
            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            Xamarin.FormsMaps.Init(this, savedInstanceState);
            LoadApplication(new App());
            if (App.Current.RequestedTheme == OSAppTheme.Dark)
            {
                SetStatusBarColor(Android.Graphics.Color.Black);
                if (Build.VERSION.SdkInt >= BuildVersionCodes.R)
                {
                    Window.InsetsController?.SetSystemBarsAppearance(0,
                        (int)WindowInsetsControllerAppearance.LightStatusBars);
                } else
                {
                    Window.DecorView.SystemUiVisibility = StatusBarVisibility.Hidden;
                }
            }
            else
            {
                SetStatusBarColor(Android.Graphics.Color.White);
                if (Build.VERSION.SdkInt >= BuildVersionCodes.R)
                {
                    Window.InsetsController?.SetSystemBarsAppearance((int)WindowInsetsControllerAppearance.LightStatusBars,
                        (int)WindowInsetsControllerAppearance.LightStatusBars);
                } else
                {
                    Window.DecorView.SystemUiVisibility = StatusBarVisibility.Visible;
                }
            }
            
        }

        public override void OnRequestPermissionsResult(int requestCode, string[] permissions,
            Permission[] grantResults)
        {
            Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);

            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }
        
        
    }
}