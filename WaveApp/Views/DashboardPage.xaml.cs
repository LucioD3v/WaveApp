using Microsoft.Maui.Controls;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;

namespace WaveApp.Views;

public partial class DashboardPage : ContentPage , INotifyPropertyChanged
{
    public event PropertyChangedEventHandler PropertyChanged;

    protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

    private readonly Random _random = new Random();
    private Timer _updateTimer;
    private readonly List<BuoyData> _buoys;
    private readonly List<AIRecommendation> _aiRecommendations;
    private readonly List<string> _weatherConditions;
    private int _currentRecommendationSet = 0;
    private DateTime _lastUpdate;

    // Base values for realistic fluctuations
    private double _baseTotalEnergy = 247.8;
    private double _basePowerOutput = 18.4;
    private int _updateCounter = 0;

    private double _totalEnergy;
    public double TotalEnergy
    {
        get => _totalEnergy;
        set
        {
            if (_totalEnergy != value)
            {
                _totalEnergy = value;
                OnPropertyChanged();
            }
        }
    }

    private string _energyTrend;
    public string EnergyTrend
    {
        get => _energyTrend;
        set
        {
            if (_energyTrend != value)
            {
                _energyTrend = value;
                OnPropertyChanged();
            }
        }
    }

    private double _currentPower;
    public double CurrentPower
    {
        get => _currentPower;
        set
        {
            if (_currentPower != value)
            {
                _currentPower = value;
                OnPropertyChanged();
            }
        }
    }

    private string _powerTrend;
    public string PowerTrend
    {
        get => _powerTrend;
        set
        {
            if (_powerTrend != value)
            {
                _powerTrend = value;
                OnPropertyChanged();
            }
        }
    }

    private string _fleetStatusSummary;
    public string FleetStatusSummary
    {
        get => _fleetStatusSummary;
        set
        {
            if (_fleetStatusSummary != value)
            {
                _fleetStatusSummary = value;
                OnPropertyChanged();
            }
        }
    }

    // In code-behind
    private bool _showStack1 = true;
    public bool ShowStack1
    {
        get => _showStack1;
        set { _showStack1 = value; OnPropertyChanged(); OnPropertyChanged(nameof(ShowStack2)); }
    }
    public bool ShowStack2 => !ShowStack1;

    private void OnToggleClicked(object sender, EventArgs e)
    {
        ShowStack1 = !ShowStack1;
    }

    public ObservableCollection<BuoyData> Buoys { get; set; }

    public ObservableCollection<ChartBarData> ChartBars { get; set; } = new ObservableCollection<ChartBarData>();

    private void InitializeChartData()
    {
        var days = new[] { "Mon", "Tue", "Wed", "Thu", "Fri", "Sat", "Sun" };
        var colors = new[]
        {
        "PrimaryBlue",
        "AccentGreen",
        "PrimaryBlue",
        "AccentGreen",
        "PrimaryBlue",
        "AccentGreen",
        "WarningOrange"
        };

        // Clear existing data first
        ChartBars.Clear();

        for (int i = 0; i < 7; i++)
        {
            var baseValue = 110 + (_random.NextDouble() - 0.5) * 60;
            ChartBars.Add(new ChartBarData
            {
                Day = days[i],
                Color = colors[i],
                Value = Math.Max(120, baseValue)
            });
        }
    }

    public DashboardPage()
    {
        InitializeComponent();
        InitializeChartData();

        _buoys = InitializeBuoys();
        Buoys = new ObservableCollection<BuoyData>(_buoys);

        _aiRecommendations = InitializeAIRecommendations();
        _weatherConditions = InitializeWeatherConditions();
        _lastUpdate = DateTime.Now;

        // Set initial values
        TotalEnergy = _baseTotalEnergy;
        CurrentPower = _basePowerOutput;
        UpdateTrendStrings();

        // Set the binding context
        BindingContext = this;

        // Start real-time updates
        _updateTimer = new Timer(UpdateDashboardData, null, TimeSpan.Zero, TimeSpan.FromSeconds(2));
    }

    private void UpdateTrendStrings()
    {
        // Calculate trends based on recent changes
        var energyChange = TotalEnergy - _baseTotalEnergy;
        var energyPercentChange = (energyChange / _baseTotalEnergy) * 100;
        var powerChange = CurrentPower - _basePowerOutput;

        EnergyTrend = $"📈 {energyPercentChange:+0.0;-0.0;0}% today | {energyChange:+0.0;-0.0;0} kWh/hr";
        PowerTrend = $"🔥 Peak: {CurrentPower * 1.2:0.0} kW | Avg: {CurrentPower * 0.85:0.0} kW";
    }

    private List<BuoyData> InitializeBuoys()
    {
        return new List<BuoyData>
        {
            new BuoyData
            {
                Id = "WB-001",
                Name = "Alpha",
                Status = BuoyStatus.Active,
                BaseEnergy = 45.2,
                CurrentEnergy = 45.2,
                BaseEfficiency = 94,
                CurrentEfficiency = 94,
                WaveHeight = 2.1,
                LastMaintenance = DateTime.Now.AddDays(-45)
            },
            new BuoyData
            {
                Id = "WB-002",
                Name = "Beta",
                Status = BuoyStatus.Active,
                BaseEnergy = 38.7,
                CurrentEnergy = 38.7,
                BaseEfficiency = 87,
                CurrentEfficiency = 87,
                WaveHeight = 1.8,
                LastMaintenance = DateTime.Now.AddDays(-32)
            },
            new BuoyData
            {
                Id = "WB-003",
                Name = "Gamma",
                Status = BuoyStatus.Maintenance,
                BaseEnergy = 0,
                CurrentEnergy = 0,
                BaseEfficiency = 0,
                CurrentEfficiency = 0,
                WaveHeight = 0,
                LastMaintenance = DateTime.Now.AddDays(-1)
            },
            new BuoyData
            {
                Id = "WB-004",
                Name = "Delta",
                Status = BuoyStatus.Active,
                BaseEnergy = 52.1,
                CurrentEnergy = 52.1,
                BaseEfficiency = 96,
                CurrentEfficiency = 96,
                WaveHeight = 2.3,
                LastMaintenance = DateTime.Now.AddDays(-28)
            },
            new BuoyData
            {
                Id = "WB-005",
                Name = "Echo",
                Status = BuoyStatus.Active,
                BaseEnergy = 41.8,
                CurrentEnergy = 41.8,
                BaseEfficiency = 89,
                CurrentEfficiency = 89,
                WaveHeight = 1.9,
                LastMaintenance = DateTime.Now.AddDays(-38)
            },
            new BuoyData
            {
                Id = "WB-006",
                Name = "Foxtrot",
                Status = BuoyStatus.Warning,
                BaseEnergy = 28.3,
                CurrentEnergy = 28.3,
                BaseEfficiency = 72,
                CurrentEfficiency = 72,
                WaveHeight = 1.6,
                LastMaintenance = DateTime.Now.AddDays(-67)
            }
        };
    }

    private List<AIRecommendation> InitializeAIRecommendations()
    {
        return new List<AIRecommendation>
        {
            // Set 1 - Current recommendations
            new AIRecommendation
            {
                Type = RecommendationType.Positioning,
                Title = "🎯 Positioning Optimization",
                Description = "Wave conditions in Zone A show 18% higher energy potential. Consider repositioning Buoy Beta for optimal capture.",
                Impact = "💡 Potential gain: +6.8 kWh/day",
                Priority = Priority.Medium
            },
            new AIRecommendation
            {
                Type = RecommendationType.Maintenance,
                Title = "🔧 Predictive Maintenance",
                Description = "Buoy Foxtrot shows declining efficiency. Sensor calibration recommended within 72 hours to prevent 15% energy loss.",
                Impact = "⏰ Schedule maintenance: High priority",
                Priority = Priority.High
            },
            new AIRecommendation
            {
                Type = RecommendationType.Weather,
                Title = "🌊 Weather Optimization",
                Description = "Storm system approaching in 48 hours. High wave activity predicted - adjust safety protocols and expect 25% energy increase.",
                Impact = "⚡ Expected boost: +45 kWh over 3 days",
                Priority = Priority.Medium
            },

            // Set 2 - Alternative recommendations
            new AIRecommendation
            {
                Type = RecommendationType.Performance,
                Title = "⚡ Performance Enhancement",
                Description = "Firmware update v2.4.1 available for Delta unit. Installation will increase energy conversion efficiency by 5%.",
                Impact = "🚀 Performance gain: +2.6 kWh/day",
                Priority = Priority.Low
            },
            new AIRecommendation
            {
                Type = RecommendationType.Expansion,
                Title = "📈 Fleet Expansion",
                Description = "Sector 7 shows optimal wave patterns for new buoy deployment. ROI analysis suggests 18-month payback period.",
                Impact = "💰 Additional capacity: +35 kWh/day",
                Priority = Priority.Low
            },
            new AIRecommendation
            {
                Type = RecommendationType.Optimization,
                Title = "🔄 Load Balancing",
                Description = "Energy storage optimization detected. Redistribute load between Alpha and Delta for 8% efficiency improvement.",
                Impact = "⚡ System efficiency: +3.2 kWh/day",
                Priority = Priority.Medium
            },

            // Set 3 - Advanced recommendations
            new AIRecommendation
            {
                Type = RecommendationType.Safety,
                Title = "🛡️ Safety Protocol",
                Description = "Unusual current patterns detected near Echo buoy. Implement enhanced monitoring for next 24 hours.",
                Impact = "🔍 Risk mitigation: Prevent potential damage",
                Priority = Priority.High
            },
            new AIRecommendation
            {
                Type = RecommendationType.Efficiency,
                Title = "🎯 Efficiency Boost",
                Description = "Tidal patterns analysis suggests optimal operating window. Adjust power extraction timing for 12% gain.",
                Impact = "⏰ Peak efficiency: 6 AM - 10 AM window",
                Priority = Priority.Medium
            },
            new AIRecommendation
            {
                Type = RecommendationType.Analytics,
                Title = "📊 Data Insights",
                Description = "Machine learning model identified new wave pattern correlation. Update algorithms for better prediction accuracy.",
                Impact = "🧠 Prediction accuracy: +15% improvement",
                Priority = Priority.Low
            }
        };
    }

    private List<string> InitializeWeatherConditions()
    {
        return new List<string>
        {
            "Clear skies, moderate waves",
            "Partly cloudy, increasing swell",
            "Storm approaching, high waves expected",
            "Calm conditions, optimal for maintenance",
            "Windy conditions, peak energy potential",
            "Fog advisory, reduced visibility",
            "High pressure system, stable conditions"
        };
    }

    private void UpdateDashboardData(object state)
    {
        MainThread.BeginInvokeOnMainThread(() =>
        {
            try
            {
                _updateCounter++;
                UpdateHeaderSection();
                UpdateEnergyOverview();
                UpdateBuoyData();

                // Update chart every update (2 seconds) for smooth animation
                UpdatePerformanceChart();
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error updating dashboard: {ex.Message}");
            }
        });
    }

    private void UpdateHeaderSection()
    {
        _lastUpdate = DateTime.Now;
        // Header updates are handled by individual component updates
    }

    private void UpdateEnergyOverview()
    {
        // Update total energy with realistic fluctuations
        var energyVariation = (_random.NextDouble() - 0.5) * 8; // ±4 kWh variation
        TotalEnergy = Math.Max(200, _baseTotalEnergy + energyVariation);

        // Update power output
        var powerVariation = (_random.NextDouble() - 0.5) * 6; // ±3 kW variation
        CurrentPower = Math.Max(12, _basePowerOutput + powerVariation);

        // Update trend strings
        UpdateTrendStrings();

        // Notify UI of changes
        OnPropertyChanged(nameof(TotalEnergy));
        OnPropertyChanged(nameof(CurrentPower));
        OnPropertyChanged(nameof(EnergyTrend));
        OnPropertyChanged(nameof(PowerTrend));

        // Calculate and update derived metrics (for future use)
        var dailyGrowth = 12.5 + (_random.NextDouble() - 0.5) * 3; // 12.5% ± 1.5%
        var hourlyRate = 3.2 + (_random.NextDouble() - 0.5) * 0.8; // 3.2 ± 0.4 kWh/hr
        var peakPower = Math.Max(CurrentPower, 22.1 + (_random.NextDouble() - 0.5) * 2);
        var avgPower = CurrentPower * 0.85; // Average is typically 85% of current
    }

    private void UpdateBuoyData()
    {
        // Simulate wave patterns that affect all buoys similarly
        var baseWaveHeight = 1.5 + (Math.Sin(_updateCounter * 0.15) * 0.8);

        for (int i = 0; i < _buoys.Count; i++)
        {
            var buoy = _buoys[i];
            if (buoy.Status == BuoyStatus.Active)
            {
                // Each buoy has slightly different response to waves
                var buoyFactor = 0.8 + (_random.NextDouble() * 0.4);

                // Update wave height with base pattern + individual variation
                buoy.WaveHeight = Math.Max(0.5, baseWaveHeight * buoyFactor + (_random.NextDouble() - 0.5) * 0.3);

                // Energy production is proportional to wave height squared
                var energyFactor = Math.Pow(buoy.WaveHeight / 2.0, 2);
                buoy.CurrentEnergy = buoy.BaseEnergy * energyFactor * (0.9 + (_random.NextDouble() * 0.2));

                // Efficiency depends on maintenance age and current conditions
                var daysSinceMaintenance = (DateTime.Now - buoy.LastMaintenance).TotalDays;
                var maintenanceFactor = Math.Max(0.7, 1.0 - (daysSinceMaintenance / 100.0));
                buoy.CurrentEfficiency = (int)(buoy.BaseEfficiency * maintenanceFactor * (0.95 + (_random.NextDouble() * 0.1)));
            }
            else if (buoy.Status == BuoyStatus.Warning)
            {
                // Warning buoys have more unstable performance
                buoy.WaveHeight = baseWaveHeight * (0.6 + (_random.NextDouble() * 0.3));
                buoy.CurrentEnergy = buoy.BaseEnergy * (0.4 + (_random.NextDouble() * 0.3));
                buoy.CurrentEfficiency = (int)(buoy.BaseEfficiency * (0.5 + (_random.NextDouble() * 0.3)));

                // 5% chance to return to normal
                if (_random.Next(0, 20) == 0)
                {
                    //buoy.Status = BuoyStatus.Active;
                }
            }

            // Update the item in the observable collection
            Buoys[i] = buoy;
        }

        // Update the total system energy based on buoy performance
        _baseTotalEnergy = _buoys.Where(b => b.Status == BuoyStatus.Active || b.Status == BuoyStatus.Warning)
                                .Sum(b => b.CurrentEnergy);
        _basePowerOutput = _baseTotalEnergy * 0.075; // Rough conversion to kW

        // Update the fleet status summary
        var activeCount = Buoys.Count(b => b.Status == BuoyStatus.Active);
        var warningCount = Buoys.Count(b => b.Status == BuoyStatus.Warning);
        var maintenanceCount = Buoys.Count(b => b.Status == BuoyStatus.Maintenance);
        FleetStatusSummary = $"✅ {activeCount} Active | ⚠️ {warningCount} Warning | 🔧 {maintenanceCount} Maintenance";

        // Notify UI that base values changed
        OnPropertyChanged(nameof(TotalEnergy));
        OnPropertyChanged(nameof(CurrentPower));
        OnPropertyChanged(nameof(FleetStatusSummary));
    }

    private void UpdateAIRecommendations()
    {
        _currentRecommendationSet = (_currentRecommendationSet + 1) % 3;

        // In a real implementation, you would update the AI recommendations
        // displayed in the UI based on current conditions and analysis
        var currentRecommendations = _aiRecommendations
            .Skip(_currentRecommendationSet * 3)
            .Take(3)
            .ToList();

        // Update recommendation logic would go here
        // For now, the XAML shows static professional examples
    }

    private void UpdatePerformanceChart()
{
    for (int i = 0; i < ChartBars.Count; i++)
    {
        // Apply small fluctuations (±5-15 kWh)
        var fluctuation = (_random.NextDouble() - 0.5) * 30;
        var newValue = Math.Max(120, ChartBars[i].Value + fluctuation);
        
        // Direct assignment to trigger property change
        ChartBars[i].Value = newValue;
    }

    // Update the weekly total display
    var total = ChartBars.Sum(b => b.Value);
    WeeklyTotalText = $"📈 Weekly Total: {total:F0} kWh | Average: {total/7:F0} kWh/day";
}

    private string _weeklyTotalText;
    public string WeeklyTotalText
    {
        get => _weeklyTotalText;
        set
        {
            _weeklyTotalText = value;
            OnPropertyChanged();
        }
    }

    private void UpdateSystemStatus()
    {
        // Update system status indicators
        var networkStatus = _random.Next(95, 101); // 95-100% connectivity
        var dataTransmitted = 2.4 + (_random.NextDouble() * 0.3); // 2.4-2.7 TB

        // Generate occasional status changes
        if (_random.Next(0, 10) == 0) // 10% chance
        {
            // Simulate status change - could update maintenance schedules,
            // firmware availability, etc.
        }
    }

    private async void OnInfoIconClicked(object sender, EventArgs e)
    {
        var weatherCondition = _weatherConditions[_random.Next(_weatherConditions.Count)];
        var activeBuoys = _buoys.Count(b => b.Status == BuoyStatus.Active);
        var totalEfficiency = _buoys.Where(b => b.Status == BuoyStatus.Active)
                                   .Average(b => b.CurrentEfficiency);

        await DisplayAlert("WaveForce Energy System Info",
            $"📊 Mathematical Foundation:\n" +
            $"P = (ρ · g² · H² · T) / (64π)\n\n" +
            $"Where:\n" +
            $"ρ: Water density (~1025 kg/m³)\n" +
            $"g: Gravity acceleration (9.81 m/s²)\n" +
            $"H: Significant wave height\n" +
            $"T: Wave period\n\n" +
            $"🤖 AI Enhancement Features:\n" +
            $"• Predictive maintenance algorithms\n" +
            $"• Weather pattern analysis\n" +
            $"• Optimal positioning calculations\n" +
            $"• Performance optimization\n" +
            $"• Real-time efficiency monitoring\n\n" +
            $"📈 Current System Status:\n" +
            $"• Active Buoys: {activeBuoys}/6\n" +
            $"• Average Efficiency: {totalEfficiency:F1}%\n" +
            $"• Weather: {weatherCondition}\n" +
            $"• Last Update: {_lastUpdate:HH:mm:ss}",
            "Close");
    }

    // Simulate different operational modes
    public void SimulateStormConditions()
    {
        foreach (var buoy in _buoys.Where(b => b.Status == BuoyStatus.Active))
        {
            buoy.WaveHeight *= 1.5; // Increase wave height
            buoy.CurrentEnergy *= 1.25; // 25% energy increase
        }
        _basePowerOutput *= 1.3; // 30% power increase during storms
    }

    public void SimulateCalmConditions()
    {
        foreach (var buoy in _buoys.Where(b => b.Status == BuoyStatus.Active))
        {
            buoy.WaveHeight *= 0.7; // Decrease wave height
            buoy.CurrentEnergy *= 0.8; // 20% energy decrease
        }
        _basePowerOutput *= 0.75; // 25% power decrease during calm
    }

    public void SimulateMaintenanceCompletion(string buoyId)
    {
        var buoy = _buoys.FirstOrDefault(b => b.Id == buoyId);
        if (buoy != null && buoy.Status == BuoyStatus.Maintenance)
        {
            buoy.Status = BuoyStatus.Active;
            buoy.CurrentEnergy = buoy.BaseEnergy;
            buoy.CurrentEfficiency = buoy.BaseEfficiency;
            buoy.LastMaintenance = DateTime.Now;
        }
    }

    protected override void OnDisappearing()
    {
        base.OnDisappearing();
        _updateTimer?.Dispose();
    }
}

// Enhanced Data Models
public class BuoyData : INotifyPropertyChanged
{
    public event PropertyChangedEventHandler PropertyChanged;

    private double _currentEnergy;
    private int _currentEfficiency;
    private double _waveHeight;
    private BuoyStatus _status;

    public string Id { get; set; }
    public string Name { get; set; }

    public BuoyStatus Status
    {
        get => _status;
        set
        {
            if (_status != value)
            {
                _status = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(StatusSymbol));
            }
        }
    }

    public string StatusSymbol => Status switch
    {
        BuoyStatus.Active => "🟢",
        BuoyStatus.Warning => "🟡",
        BuoyStatus.Maintenance => "🔴",
        BuoyStatus.Offline => "⚫",
        _ => "⚪"
    };

    public double BaseEnergy { get; set; }

    public double CurrentEnergy
    {
        get => _currentEnergy;
        set
        {
            if (_currentEnergy != value)
            {
                _currentEnergy = value;
                OnPropertyChanged();
            }
        }
    }

    public int BaseEfficiency { get; set; }

    public int CurrentEfficiency
    {
        get => _currentEfficiency;
        set
        {
            if (_currentEfficiency != value)
            {
                _currentEfficiency = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(EfficiencyStatus));
            }
        }
    }

    public double WaveHeight
    {
        get => _waveHeight;
        set
        {
            if (_waveHeight != value)
            {
                _waveHeight = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(EfficiencyStatus));
            }
        }
    }

    //public string EfficiencyStatus => $"Efficiency: {CurrentEfficiency}% | 🌊 Wave: {WaveHeight:0.0}m";

    public DateTime LastMaintenance { get; set; }

    public List<string> Sensors { get; set; } = new List<string>
    {
        "Wave Height Sensor",
        "Power Converter",
        "Position GPS",
        "Communication Module",
        "Battery Monitor"
    };

    public string StatusColor => Status switch
    {
        BuoyStatus.Active => "#00FF88", // AccentGreen
        BuoyStatus.Warning => "#FF6B35", // WarningOrange
        BuoyStatus.Maintenance => "#FF3B30", // DangerRed
        _ => "#8E9AAF" // TextSecondary
    };

    public string StatusDetailColor => Status switch
    {
        BuoyStatus.Active => "#8E9AAF", // TextSecondary
        BuoyStatus.Warning => "#FF6B35", // WarningOrange
        BuoyStatus.Maintenance => "#FF6B35", // WarningOrange
        _ => "#8E9AAF" // TextSecondary
    };

    public string EfficiencyStatus => Status switch
    {
        BuoyStatus.Maintenance => "🔧 Scheduled Maintenance",
        BuoyStatus.Warning => $"Efficiency: {CurrentEfficiency}% | ⚠️ Check sensors",
        _ => $"Efficiency: {CurrentEfficiency}% | 🌊 Wave: {WaveHeight:F1}m"
    };

    protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}

public class ChartBarData : INotifyPropertyChanged
{
    public event PropertyChangedEventHandler PropertyChanged;

    private double _height;

    public string Day { get; set; }
    public string Color { get; set; }
    private double _value;
    public double Value
    {
        get => _value;
        set
        {
            _value = value;
            OnPropertyChanged();
            OnPropertyChanged(nameof(ValueText));
            OnPropertyChanged(nameof(Height)); // Add this line
        }
    }

    public double Height => Value / 2.5; // Adjust this factor to control bar height scaling

    public string ValueText => Value.ToString("F0");

    protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}

public class AIRecommendation
{
    public RecommendationType Type { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public string Impact { get; set; }
    public Priority Priority { get; set; }
    public DateTime Created { get; set; } = DateTime.Now;
}

public enum BuoyStatus
{
    Active,
    Warning,
    Maintenance,
    Offline
}

public enum RecommendationType
{
    Positioning,
    Maintenance,
    Weather,
    Performance,
    Expansion,
    Optimization,
    Safety,
    Efficiency,
    Analytics
}

public enum Priority
{
    Low,
    Medium,
    High,
    Critical
}