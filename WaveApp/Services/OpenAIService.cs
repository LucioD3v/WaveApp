using System.Text;
using System.Text.Json;
using System.Net.Http.Headers;

namespace WaveApp.Services
{
    public static class OpenAIService
    {
        private const string ApiKey = "TU_API_KEY_OPENAI";
        private const string Endpoint = "https://api.openai.com/v1/chat/completions";

        public static async Task<string> AskAsync(string question)
        {
            using var client = new HttpClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", ApiKey);

            var requestBody = new
            {
                model = "gpt-3.5-turbo",
                messages = new[]
                {
                    new { role = "user", content = question }
                }
            };

            var content = new StringContent(JsonSerializer.Serialize(requestBody), Encoding.UTF8, "application/json");
            var response = await client.PostAsync(Endpoint, content);
            response.EnsureSuccessStatusCode();

            var json = await response.Content.ReadAsStringAsync();
            using var doc = JsonDocument.Parse(json);
            return doc.RootElement.GetProperty("choices")[0].GetProperty("message").GetProperty("content").GetString();
        }
    }
}
