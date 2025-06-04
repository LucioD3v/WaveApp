using CommunityToolkit.Mvvm.ComponentModel;
using Microsoft.Maui.Dispatching;
using System.Collections.ObjectModel;
using System.Timers;
using WaveApp.Models;

namespace WaveApp.ViewModels;

public partial class MapViewModel : ObservableObject
{
    private readonly Random _random = new();
    private readonly IDispatcherTimer _waveTimer; // Use IDispatcherTimer instead of DispatcherTimer

    public ObservableCollection<Buoy> Buoys { get; }

    public MapViewModel()
    {
        Buoys = new ObservableCollection<Buoy>
        {
            new Buoy
            {
                Name = "Buoy 1",
                Description = "Boya de monitoreo ambiental cerca del Golden Gate.",
                Latitude = 37.8199,
                Longitude = -122.4783,
                Status = "Activa",
                MaxCapacity = 100 // Capacidad máxima en kW
            },
            new Buoy
            {
                Name = "Buoy 2",
                Description = "Boya de medición de corrientes",
                Latitude = 34.0522,
                Longitude = -118.2437,
                Status = "En Mantenimiento",
                MaxCapacity = 80
            },
            new Buoy
            {
                Name = "Buoy 3",
                Description = "Boya de monitoreo de temperatura",
                Latitude = 40.7128,
                Longitude = -74.0060,
                Status = "Desactiva",
                MaxCapacity = 50
            }
        };

        // Initialize the IDispatcherTimer
        _waveTimer = Application.Current.Dispatcher.CreateTimer();
        _waveTimer.Interval = TimeSpan.FromSeconds(2);
        _waveTimer.Tick += (s, e) => SimulateEnergyGeneration();
    }

    public void SimulateEnergyGeneration()
    {
        foreach (var buoy in Buoys)
        {
            if (buoy.Status == "Activa")
            {
                // Simula la intensidad de las olas (0-1)
                var waveIntensity = _random.NextDouble();
                buoy.CurrentEnergy = waveIntensity * buoy.MaxCapacity;
            }
        }
    }

    public void StopWaveSimulation()
    {
        _waveTimer.Stop();
    }

    public void StartWaveSimulation()
    {
        _waveTimer.Start();
    }
}
