using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using WaveApp.Services;

namespace WaveApp.ViewModels
{
    public partial class DashboardViewModel : ObservableObject
    {
        [ObservableProperty] private string weatherDescription;
        [ObservableProperty] private string temperature;
        [ObservableProperty] private string weatherIcon;
        [ObservableProperty] private string userQuestion;
        [ObservableProperty] private string openAIResponse;
        [ObservableProperty] private bool isBusy;

        public IRelayCommand AskOpenAICommand { get; }

        public DashboardViewModel()
        {
            AskOpenAICommand = new AsyncRelayCommand(AskOpenAIAsync, () => !IsBusy);
            LoadWeather();
        }

        private async void LoadWeather()
        {
            var weather = await WeatherService.GetWeatherAsync();
            WeatherDescription = weather.Description;
            Temperature = $"{weather.Temperature}°C";
            WeatherIcon = weather.IconUrl;
        }

        private async Task AskOpenAIAsync()
        {
            if (string.IsNullOrWhiteSpace(UserQuestion)) return;
            IsBusy = true;
            OpenAIResponse = string.Empty;
            OpenAIResponse = await OpenAIService.AskAsync(UserQuestion);
            IsBusy = false;
        }
    }
}
