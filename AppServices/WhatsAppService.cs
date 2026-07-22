using Newtonsoft.Json;
using System.Text;

public class WhatsAppService
{
    private static readonly HttpClient _httpClient = new HttpClient();

    public async Task<bool> SendWhatsAppAsync(string fullUrl)
    {
        try
        {
            using var emptyContent = new StringContent(string.Empty); 
            HttpResponseMessage response = await _httpClient.PostAsync(fullUrl, emptyContent);

            // Return true if successful
            return response.IsSuccessStatusCode;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error sending webhook: {ex.Message}");
            return false;
        }
    }
}
