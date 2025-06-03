namespace WaveApp.Views;

public partial class OpenSeaMap : ContentPage
{
	public OpenSeaMap()
	{
		InitializeComponent();

        OpenSeaMapView.Source = "https://map.openseamap.org/?lang=es";
    }
}