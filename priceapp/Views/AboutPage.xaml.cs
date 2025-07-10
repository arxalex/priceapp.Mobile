namespace priceapp.Views;


public partial class AboutPage
{
    public AboutPage()
    {
        InitializeComponent();
        Label.Text = "Версія: " + VersionTracking.CurrentVersion + " (" + VersionTracking.CurrentBuild + ")";
    }

    private async void ImageButton_OnClicked(object sender, EventArgs e)
    {
        await Navigation.PopAsync();
    }
}