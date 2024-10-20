using Serilog;
using Newtonsoft.Json;

namespace AlfredAPI.AlfredAPI.Services
{
    public class Auth
    {
        private static ILogger logger = Log.ForContext("Title", "Services @ Auth");
        private static Oauth? _cache;

        public static async Task<string?> GetToken()
        {
            if (_cache != null) return _cache.AccessToken;

            await RefeshTokenAsync();
            RunRefreshAsync((_cache?.ExpiresIn ?? 14400) - 5 * 600); // approximately 3 hours and 10 minutes
            return _cache?.AccessToken;
        }

        private static void RunRefreshAsync(int delay)
        {
            Task.Run(async () =>
            {
                while (true)
                {
                    await Task.Delay(delay * 1000);
                    await RefeshTokenAsync();
                }
            });
        }

        public static async Task RefeshTokenAsync()
        {
            logger.Debug("Generating New Token...");
            var request = new HttpRequestMessage(HttpMethod.Post, "https://account-public-service-prod03.ol.epicgames.com/account/api/oauth/token")
            { Content = new FormUrlEncodedContent(new Dictionary<string, string> { { "grant_type", "client_credentials" }, { "token_type", "eg1" } }) };

            request.Headers.Add("Authorization", "basic MzQ0NmNkNzI2OTRjNGE0NDg1ZDgxYjc3YWRiYjIxNDE6OTIwOWQ0YTVlMjVhNDU3ZmI5YjA3NDg5ZDMxM2I0MWE=");
            var response = await Global.Client.SendAsync(request);

            var content = await response.Content.ReadAsStringAsync();
            var data = JsonConvert.DeserializeObject<Oauth>(content);
            if (data == null) return;
            _cache = data;
        }

        public class Oauth
        {
            [JsonProperty("access_token")] public string? AccessToken { get; set; }
            [JsonProperty("expires_in")] public int ExpiresIn { get; set; }
        }
    }
}