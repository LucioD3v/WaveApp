using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;

namespace WaveApp.ViewModels;

public partial class MapViewModel : ObservableObject
{
    public ObservableCollection<Buoy> Buoys { get; }

    public MapViewModel()
    {
        Buoys = new ObservableCollection<Buoy>
        {
            new Buoy
            {
                Name = "Buoy 1: ",
                Description = "Boya de monitoreo ambiental cerca del Golden Gate.",
                Latitude = 37.8199,
                Longitude = -122.4783
            },
            new Buoy
            {
                Name = "Buoy 2",
                Description = "Boya de medición de corrientes",
                Latitude = 34.0522,
                Longitude = -118.2437
            },
            new Buoy
            {
                Name = "Buoy 3",
                Description = "Boya de monitoreo de temperatura",
                Latitude = 40.7128,
                Longitude = -74.0060
            }
        };
    }
}

public class Buoy
{
    public string Name { get; set; }
    public string Description { get; set; }
    public double Latitude { get; set; }
    public double Longitude { get; set; }
}
