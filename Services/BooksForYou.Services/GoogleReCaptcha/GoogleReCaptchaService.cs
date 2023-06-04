namespace BooksForYou.Services.GoogleReCaptcha
{
    using System;
    using System.Net;
    using System.Net.Http;
    using System.Threading.Tasks;

    using Microsoft.Extensions.Options;
    using Newtonsoft.Json;

    public class GoogleReCaptchaService
    {
        private IOptionsMonitor<GoogleReCaptchaConfig> _config;

        public GoogleReCaptchaService(IOptionsMonitor<GoogleReCaptchaConfig> config)
        {
            _config = config;
        }

        public async Task<bool> VerifyToken(string token)
        {
            try
            {
                var url = $"https://www.google.com/recaptcha/api/siteverify?secret={_config.CurrentValue.SecretKey}&response={token}";

                using (var client = new HttpClient())
                {
                    var httpResult = await client.GetAsync(url);
                    if (httpResult.StatusCode != HttpStatusCode.OK)
                    {
                        return false;
                    }

                    var responseString = await httpResult.Content.ReadAsStringAsync();

                    var googleResult = JsonConvert.DeserializeObject<GoogleReCaptchaResponse>(responseString);

                    return googleResult.Success && googleResult.Score >= 0.5;
                }
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}
