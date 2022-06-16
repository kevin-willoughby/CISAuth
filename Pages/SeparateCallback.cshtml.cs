using CIS2Auth.Dto;
using CIS2Auth.Jwt;
using CIS2Auth.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace CIS2Auth.Pages
{
    public class SeparateCallbackModel : PageModel
    {
        private static HttpClient client = new HttpClient();

        private const string PUBLIC_KEY = @"-----BEGIN PUBLIC KEY-----
MIICIjANBgkqhkiG9w0BAQEFAAOCAg8AMIICCgKCAgEAxfJmZuteZpKk4TjrEE/I
yFrb2csrlxhvz7TMP3V7oNP42C52JUex9yw1njoQl4xjB9SSSobHyEr06EUsC3Rt
Xg9mh8X5DRVyWp1RIu2RSW7y+b4Uy/xDb60pBIhRt+ZZ7VvlDtPU6WOHw6s9ewKM
DA5rZ5hPyrR/I1fC2tBcp4pHN6MSjQJ6xTVYsHT7TJ+M2ZtxSlLLJw35vHEdYV94
CVTsBuVyvkvpIzCTwGxOCkdNefxiHep2gizj8nMFMV5mGfmLfO/CLFYOY0osfSm7
JtiJNr5QujNysh3WuJndZ45CFiVY2/ALJ8zqwb+bVOBUq+zrhFk4khObOYwjfElm
9Kp9omAR68+mqgqAiE0+q0docLfvAoRDcOO56w5Ak14ubXMt8zymsaJoyGiCvWMC
42YG+AndRZlC1aVasDMS6H93v6I93JuSo0Qyg3QsWk/UCqcIDwEv2w0b6tAUwYiU
u0V7lxH+g9oOmO7kknCKMUD9De01+WbBwjIly1A9f/H64eEnRp0jegJDOG/t/IkA
NKI/HKzmXDZqhN+dTRjQKILvT3N/Jw3957AYOPdh7D6UgupLqg6g0BMMWTuEixlj
FaqS4wDSM/sDHq/51hxfcK67dOox0VmAL6yO5H4VXSkAB1rVbRKLWxExx16Qiqn0
qkrDrRcvpM9dkGfsK/KIWNMCAwEAAQ==
-----END PUBLIC KEY-----";

        private const string PRIVATE_KEY = @"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";

        [BindProperty]
        public CallbackViewModel CallbackData { get; set; }
        [BindProperty]
        public AuthSettingsViewModel AuthSettings { get; set; }

        [BindProperty]
        public string HelperText =>
            $"The authenticate page has called us back with the same state as we passed along with the authenticate code.  To complete the process we have to get a token.\nThis will POST to {AuthSettings.TokenSeparateUri} {GeneratePayloadJson("code", "jwt")}";
        
        public string JWT => GenerateJWT();
        
        public string PrivateKey => PRIVATE_KEY;
        public string PublicKey => PUBLIC_KEY;

        public string Kid => "dev-1";

        private string GenerateJWT()
        {
            var handler = new JwtHandler(AuthSettings.TokenSeparateUri, AuthSettings.SeparateApiKey, "dev-1", PRIVATE_KEY);
            return handler.generateJWT(5);
        }



        public void OnGet(string code, string state)
        {

            var hardcodedSubjectToken = "eyJ0eXAiOiJKV1QiLCJhbGciOiJSUzI1NiIsImtpZCI6ImlkZW50aXR5LXNlcnZpY2UtdGVzdHMtMSJ9.eyJhdF9oYXNoIjoidGZfLWxxcHEzNmx3TzdXbVNCSUo2USIsInN1YiI6Ijc4NzgwNzQyOTUxMSIsImF1ZGl0VHJhY2tpbmdJZCI6IjkxZjY5NGU2LTM3NDktNDJmZC05MGIwLWMzMTM0YjBkOThmNi0xNTQ2MzkxIiwiYW1yIjpbIk4zX1NNQVJUQ0FSRCJdLCJpc3MiOiJodHRwczovL2FtLm5oc2ludC5hdXRoLXB0bC5jaXMyLnNwaW5lc2VydmljZXMubmhzLnVrOjQ0My9vcGVuYW0vb2F1dGgyL3JlYWxtcy9yb290L3JlYWxtcy9OSFNJZGVudGl0eS9yZWFsbXMvSGVhbHRoY2FyZSIsInRva2VuTmFtZSI6ImlkX3Rva2VuIiwiYXVkIjoic29tZS1jbGllbnQtaWQiLCJjX2hhc2giOiJiYzd6ekdrQ2xDM01FaUZRM1loUEtnIiwiYWNyIjoiQUFMM19BTlkiLCJvcmcuZm9yZ2Vyb2NrLm9wZW5pZGNvbm5lY3Qub3BzIjoiLUk0NU5qbU1EZE1hLWFORjJzcjloQzdxRUdRIiwic19oYXNoIjoiTFBKTnVsLXdvdzRtNkRzcXhibmluZyIsImF6cCI6InNvbWUtY2xpZW50LWlkIiwiYXV0aF90aW1lIjoxNjEwNTU5ODAyLCJyZWFsbSI6Ii9OSFNJZGVudGl0eS9IZWFsdGhjYXJlIiwiZXhwIjoxNjgxODY1Nzg1LCJ0b2tlblR5cGUiOiJKV1RUb2tlbiIsImlhdCI6MTYyMTg2NTc3NSwic2VsZWN0ZWRfcm9sZWlkIjoiNTU1MjU0MjQyMTAyIn0.UVVozQz5fJDqzRta3NsbbJ6tldbFKtnPwrUHvDtGKGGYGEm1Bx1mY9QubB2HpX6yJT_dN5VGbFf-dsiqk7WV0wGXxfA3vabSf-OF68hEwed291_bmLOSkUrHbf5tLYFWAAqri3F-TzWhGGknBQ6FfttXpDeRtLodf03-jX-KeFomL_4ofLYjugiRD636Jjzt7_RdRmyaRL-sKMfIoabW6wsNO-ifAJrhyGIqRuLZB_HJuZgiHOAlLIHejJgJkvpfmsn-hPbkKKM21h4mc73WlHMISp0B07vRFYj1IXhkcE2zpRnM33eLFJqrTyWZhl5LNb6J-yI-2GnykYpqKIyvww";
            code = hardcodedSubjectToken;


            AuthSettings = new AuthSettingsViewModel();
            CallbackData = new CallbackViewModel
            {
                Code = code,
                State = state
            };
        }

        public string GetTokenPayload => GeneratePayloadJson(CallbackData.Code, GenerateJWT());

        private string GeneratePayloadJson(string code, string jwt)
        {
            return JsonConvert.SerializeObject(GeneratePayload(code,jwt), Formatting.Indented);
        }

        private Dictionary<string,string> GeneratePayload(string code, string jwt)
        {
            
            return new Dictionary<string, string>()
            {
                {"subject_token", code},
                {"client_assertion",jwt},
                {"subject_token_type","urn:ietf:params:oauth:token-type:id_token" },
                {"client_assertion_type","urn:ietf:params:oauth:client-assertion-type:jwt-bearer" },
                {"grant_type", "urn:ietf:params:oauth:grant-type:token-exchange"}

            };
        }

        

        public async Task<IActionResult> OnPost()
        {
            client.DefaultRequestHeaders.Add("cache-control", "no-cache");
            var dict = GeneratePayload(CallbackData.Code, GenerateJWT());

            var postData = new FormUrlEncodedContent(dict);
            var request = new HttpRequestMessage(HttpMethod.Post, AuthSettings.TokenSeparateUri);
            request.Content = postData;

            var response = await client.SendAsync(request);
            var responseString = await response.Content.ReadAsStringAsync();


            var json = JsonConvert.DeserializeObject<TokenResponse>(responseString);


            
            return RedirectToPage("TokenRetrievedSeparate", new
            {
                accessToken = json.access_token,
                expiresIn = json.expires_in,
                refreshToken = json.refresh_token,
                refreshTokenExpiresIn = json.refresh_token_expires_in,
                refreshCount = json.refresh_count,
                tokenType = json.token_type
            });
        }


    }
}
