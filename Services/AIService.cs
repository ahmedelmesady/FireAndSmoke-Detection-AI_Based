using System.Text.Json;
using WebApplication1.Models;
using static WebApplication1.Services.AIService;

namespace WebApplication1.Services
{
    public interface IAIService
    {
        Task<PredictResponse?> PredictAsync(byte[] imageBytes);
    }
    public class AIService : IAIService
    {
        private readonly HttpClient _httpClient;

        public AIService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<PredictResponse?> PredictAsync(byte[] imageBytes)
        {
            var content = new MultipartFormDataContent();

            content.Add(
                new ByteArrayContent(imageBytes),
                "file",
                "frame.jpg");

            var response = await _httpClient.PostAsync("predict", content);

            if (!response.IsSuccessStatusCode)
                return null;

            var json = await response.Content.ReadAsStringAsync();

            return JsonSerializer.Deserialize<PredictResponse>(
                json,
                new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });
        }
    }
}
