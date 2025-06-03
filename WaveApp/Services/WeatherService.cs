using System.Net.Http.Json;
using System.Net;
using WaveApp.Models;

namespace WaveApp.Services
{
    public static class WeatherService
    {
        private const string ApiKey = "22a32a78e31a987eb36e8efc7a9eb6cb";
        private const string City = "Cancún, MX";

        public static async Task<WeatherInfo> GetWeatherAsync()
        {
            using var client = new HttpClient();
            var encodedCity = WebUtility.UrlEncode(City);
            var url = $"https://api.openweathermap.org/data/2.5/weather?q={encodedCity}&appid={ApiKey}&units=metric&lang=es";

            try
            {
                var result = await client.GetFromJsonAsync<OpenWeatherResponse>(url);

                if (result?.main == null || result.weather == null || result.weather.Count == 0)
                {
                    return GetUnavailableWeather();
                }

                return new WeatherInfo
                {
                    Temperature = result.main.temp,
                    Description = Capitalize(result.weather[0].description),
                    IconUrl = $"https://openweathermap.org/img/wn/{result.weather[0].icon}@2x.png"
                };
            }
            catch
            {
                return GetUnavailableWeather();
            }
        }

        private static WeatherInfo GetUnavailableWeather() => new WeatherInfo
        {
            Temperature = 0,
            Description = "Not available",
            IconUrl = string.Empty
        };

        private static string Capitalize(string text)
        {
            if (string.IsNullOrWhiteSpace(text)) return text;
            return char.ToUpper(text[0]) + text.Substring(1);
        }
    }

    public class OpenWeatherResponse
    {
        public MainInfo main { get; set; }
        public List<WeatherDesc> weather { get; set; }
    }

    public class MainInfo
    {
        public double temp { get; set; }
    }

    public class WeatherDesc
    {
        public string description { get; set; }
        public string icon { get; set; }
    }
}