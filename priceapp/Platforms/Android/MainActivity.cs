using Android.App;
using Android.Content.PM;
using Android.OS;
using Android.Views;
using Microsoft.Maui.Platform;

namespace priceapp;

[Activity(Theme = "@style/Maui.SplashTheme", MainLauncher = true, LaunchMode = LaunchMode.SingleTop,
    ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation | ConfigChanges.UiMode |
                           ConfigChanges.ScreenLayout | ConfigChanges.SmallestScreenSize | ConfigChanges.Density)]
public class MainActivity : MauiAppCompatActivity
{
    protected override void OnCreate(Bundle savedInstanceState)
    {
        base.OnCreate(savedInstanceState);
    
        Platform.Init(this, savedInstanceState);
    
        if (App.Current.RequestedTheme == AppTheme.Dark)
        {
            Window.SetStatusBarColor(Colors.Black.ToPlatform());
            if (Build.VERSION.SdkInt >= BuildVersionCodes.R)
            {
                Window.InsetsController?.SetSystemBarsAppearance(0,
                    (int)WindowInsetsControllerAppearance.LightStatusBars);
            } 
            else
            {
                Window.DecorView.SystemUiVisibility = StatusBarVisibility.Hidden;
            }
        }
        else
        {
            Window.SetStatusBarColor(Colors.White.ToPlatform());
            if (Build.VERSION.SdkInt >= BuildVersionCodes.R)
            {
                Window.InsetsController?.SetSystemBarsAppearance((int)WindowInsetsControllerAppearance.LightStatusBars,
                    (int)WindowInsetsControllerAppearance.LightStatusBars);
            } 
            else
            {
                Window.DecorView.SystemUiVisibility = StatusBarVisibility.Visible;
            }
        }

    }

}