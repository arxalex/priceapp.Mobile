using System.Threading.Tasks;
using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.OS;
using Android.Widget;

namespace priceapp.Droid
{
    [Activity(Label = "priceapp",
        Icon = "@mipmap/ic_launcher",
        Theme = "@style/MainTheme.Splash",
        MainLauncher = true,
        Immersive = true,
        NoHistory = true,
        ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation,
        ScreenOrientation = ScreenOrientation.Portrait)]
    public class SplashScreenActivity : Activity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.SplashScreen);
            FindViewById<TextView>(Resource.Id.textView).Text =
                $"Версія {PackageManager.GetPackageInfo(PackageName, 0).VersionName}";
        }

        protected override void OnResume()
        {
            base.OnResume();
            var startupWork = new Task(() => { SimulateStartup(); });
            startupWork.Start();
        }

        public override void OnBackPressed()
        {
        }

        private async void SimulateStartup()
        {
            await Task.Delay(1000);
            StartActivity(new Intent(Application.Context, typeof(MainActivity)));
        }
    }
}