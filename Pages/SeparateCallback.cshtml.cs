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

        [BindProperty]
        public CallbackViewModel CallbackData { get; set; }
        [BindProperty]
        public AuthSettingsViewModel AuthSettings { get; set; }

        [BindProperty]
        public string HelperText =>
            $"The authenticate page has called us back with the same state as we passed along with the authenticate code.  To complete the process we have to get a token.\nThis will POST to {AuthSettings.TokenSeparateUri} {GeneratePayloadJson("code", "jwt")}";
        
        public string JWT => GenerateJWT();
        
        public string PrivateKey => AuthSettings.PRIVATE_KEY;
        public string PublicKey => AuthSettings.PUBLIC_KEY;

        public string Kid => AuthSettings.Kid;

        private string GenerateJWT()
        {
            var handler = new JwtHandler(AuthSettings.TokenSeparateUri, AuthSettings.SeparateApiKey, Kid, AuthSettings.PRIVATE_KEY);
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
                tokenType = json.token_type
            });
        }


    }
}
