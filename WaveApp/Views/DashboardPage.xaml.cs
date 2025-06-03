
namespace WaveApp.Views;

public partial class DashboardPage : ContentPage
{
    public DashboardPage()
    {
        InitializeComponent();
    }

    private async void OnInfoIconClicked(object sender, EventArgs e)
    {
        // Show the popup
        await DisplayAlert("Mathematical calculation of wave energy",
            "P = (ρ · g² · H² · T) / (64π)\n\n" +
            "Where:\n" +
            "ρ: Density of water (approximately 1025kg/m³)\n" +
            "g: Acceleration of gravity (9.81 m/s²)\n" +
            "H: Significant wave height\n" +
            "T: Period of the waves",
            "Close");
    }
}