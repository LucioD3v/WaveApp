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
    }
}