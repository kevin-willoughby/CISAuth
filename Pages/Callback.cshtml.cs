using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using CIS2Auth.Dto;
using CIS2Auth.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;

namespace CIS2Auth.Pages
{
    public class CallbackModel : PageModel
    {

        private static HttpClient client = new HttpClient();

        [BindProperty]
        public CallbackViewModel CallbackData { get; set; }
        [BindProperty]
        public AuthSettingsViewModel AuthSettings { get; set; }

        [BindProperty]
        public string HelperText =>
            $"The authenticate page has called us back with the same state as we passed along with the authenticate code.  To complete the process we have to get a token.\nThis will POST to {AuthSettings.TokenCombinedUri} a json document";

        public void OnGet(string code,string state)
        {
            AuthSettings = new AuthSettingsViewModel();
            CallbackData = new CallbackViewModel
            {
                Code = code,
                State = state
            };
            
            //We now will go on and attempt to authenticate the user
        }

        public async Task<IActionResult> OnPost()
        {

            
            client.DefaultRequestHeaders.Add("cache-control", "no-cache");
            var dict = new Dictionary<string, string>()
            {
                {"grant_type", "authorization_code"},
                {"client_id", AuthSettings.CombinedApiKey},
                {"client_secret", AuthSettings.CombinedApiSecret},
                {"redirect_uri", AuthSettings.RedirectCombinedUri},
                {"code", CallbackData.Code}
            };
            var postData = new FormUrlEncodedContent(dict);
            var request = new HttpRequestMessage(HttpMethod.Post, AuthSettings.TokenCombinedUri);
            request.Content = postData;
            var response = await client.SendAsync(request);
            var responseString = await response.Content.ReadAsStringAsync();

            
            var json = JsonConvert.DeserializeObject<TokenResponse>(responseString);

            return RedirectToPage("TokenRetrieved", new
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


