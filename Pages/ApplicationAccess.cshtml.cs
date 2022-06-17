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
    public class ApplicationAccessModel : PageModel
    {

        private static HttpClient client = new HttpClient();

        [BindProperty]
        public string HelperText =>
            $"To generate a bearer token in App access we POST to {AuthSettings.TokenApplicationAccessUri} {GeneratePayloadJson("jwt")}";


        [BindProperty]
        public AuthSettingsViewModel AuthSettings { get; set; }

        public void OnGet()
        {
            AuthSettings = new AuthSettingsViewModel();
        }


        public string JWT => GenerateJWT();

        public string PrivateKey => AuthSettings.PRIVATE_KEY;
        public string PublicKey => AuthSettings.PUBLIC_KEY;

        public string Kid => AuthSettings.Kid;

        private string GenerateJWT()
        {
            var handler = new JwtHandler(AuthSettings.TokenApplicationAccessUri, AuthSettings.AppAccessApiKey, Kid, AuthSettings.PRIVATE_KEY);
            return handler.generateJWT(5);
        }

        public string GetTokenPayload => GeneratePayloadJson(GenerateJWT());

        private string GeneratePayloadJson(string jwt)
        {
            return JsonConvert.SerializeObject(GeneratePayload(jwt), Formatting.Indented);
        }

        private Dictionary<string, string> GeneratePayload(string jwt)
        {

            return new Dictionary<string, string>()
            {
                {"client_assertion",jwt},
                {"client_assertion_type","urn:ietf:params:oauth:client-assertion-type:jwt-bearer" },
                {"grant_type", "client_credentials"}

            };
        }



        public async Task<IActionResult> OnPost()
        {
            client.DefaultRequestHeaders.Add("cache-control", "no-cache");
            var dict = GeneratePayload(GenerateJWT());

            var postData = new FormUrlEncodedContent(dict);
            var request = new HttpRequestMessage(HttpMethod.Post, AuthSettings.TokenApplicationAccessUri);
            request.Content = postData;

            var response = await client.SendAsync(request);
            var responseString = await response.Content.ReadAsStringAsync();


            var json = JsonConvert.DeserializeObject<TokenResponse>(responseString);

            return RedirectToPage("TokenRetrievedAppAccess", new
            {
                accessToken = json.access_token,
                expiresIn = json.expires_in,
                tokenType = json.token_type
            });
        }

    }
}
