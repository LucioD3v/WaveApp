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
        [ObservableProperty] private bool hasError;
        [ObservableProperty] private string errorMessage;

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
            HasError = false;

            if (string.IsNullOrWhiteSpace(UserQuestion))
            {
                OpenAIResponse = string.Empty;
                HasError = true;
                ErrorMessage = "Please enter a question before asking.";
                return;
            }

            IsBusy = true;
            OpenAIResponse = string.Empty;

            try
            {
                OpenAIResponse = await OpenAIService.AskAsync(UserQuestion);
            }
            catch (Exception ex)
            {
                HasError = true;
                ErrorMessage = "An error occurred while processing your request.";
            }
            finally
            {
                IsBusy = false;
            }
        }
    }
}
