using Newtonsoft.Json;

namespace CIS2Auth.Dto
{
    public class TokenResponse
    {
        public string access_token { get; set; }
        public long expires_in { get; set; }
        public string refresh_token { get; set; }
        public long refresh_token_expires_in { get; set; }
        public int refresh_count { get; set; }
        public string token_type { get; set; }
    }
}
