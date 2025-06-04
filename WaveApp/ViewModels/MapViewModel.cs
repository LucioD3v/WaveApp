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
                Name = "Buoy 1: Anemoi",
                Description = "Named after the Greek wind gods, this buoy not only generates energy but also detects and classifies microplastics in ocean currents, sending real-time alerts about water quality.",
                Latitude = 37.8199,
                Longitude = -122.4783,
                Status = "Activa",
                MaxCapacity = 100 // Capacidad máxima en kW
            },
            new Buoy
            {
                Name = "Buoy 2: Quimera",
                Description = "With its modular and adaptive design, the Chimera can reconfigure its structure to optimize energy capture in various wave conditions, acting as a hybrid between different types of marine generators.",
                Latitude = 34.0522,
                Longitude = -118.2437,
                Status = "En Mantenimiento",
                MaxCapacity = 80
            },
            new Buoy
            {
                Name = "Buoy 3: Elara",
                Description = "Named after one of Jupiter's moons, Elara is a stealthy buoy designed to operate in deep, remote waters. Its primary mission is to harvest energy from underwater currents, almost undetectable from the surface.",
                Latitude = 40.7128,
                Longitude = -74.0060,
                Status = "Desactiva",
                MaxCapacity = 50
            },
            new Buoy
            {
                Name = "Buoy 4: Kraken",
                Description = "Inspired by the legendary sea creature, the Kraken uses a system of underwater turbines that mimic natural vortices to maximize the conversion of wave energy into electricity, even in calm sea conditions.",
                Latitude = 21.17429,
                Longitude = -86.84656,
                Status = "Activa",
                MaxCapacity = 100
            },
            new Buoy
            {
                Name = "Buoy 5: Heliotrope",
                Description = "This buoy, whose name evokes a plant that turns towards the sun, optimizes its position and the angles of its energy capture devices based on wave patterns and swell direction, always seeking maximum efficiency.",
                Latitude = 24.5,
                Longitude = 79.9,
                Status = "Activa",
                MaxCapacity = 100
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
