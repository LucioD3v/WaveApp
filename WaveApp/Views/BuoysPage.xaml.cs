using System.Text;
using WaveApp.ViewModels;
using WaveApp.Models;
using System.Collections.ObjectModel;

namespace WaveApp.Views;

public partial class BuoysPage : ContentPage
{
    private readonly MapViewModel _viewModel;

    public BuoysPage()
    {
        InitializeComponent();

        // Vincular el ViewModel de MapPage
        _viewModel = new MapViewModel();
        BindingContext = _viewModel;

        // Ensure BuoyList is initialized
        BuoyList = new StackLayout(); // Replace with the actual type and initialization logic
    }

    // Add a property for BuoyList
    public StackLayout BuoyList { get; set; }

    protected override void OnAppearing()
    {
        base.OnAppearing();
        _viewModel.StartWaveSimulation();
    }

    protected override void OnDisappearing()
    {
        base.OnDisappearing();
        _viewModel.StopWaveSimulation();
    }

    private void StartBuoyAnimations()
    {
        foreach (var child in BuoyList.Children)
        {
            if (child is Frame frame)
            {
                AnimateBuoy(frame);
            }
        }
    }

    private async void AnimateBuoy(View buoyView)
    {
        while (true)
        {
            await buoyView.TranslateTo(0, -5, 500, Easing.SinInOut);
            await buoyView.TranslateTo(0, 5, 500, Easing.SinInOut);
        }
    }
}