using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using CIS2Auth.Dto;
using CIS2Auth.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;

namespace CIS2Auth.Pages
{
    public class TokenRetrievedSeparateModel : PageModel
    {

        private static HttpClient client = new HttpClient();

        [BindProperty]
        public TokenResponse Token { get; set; }


        [BindProperty]
        public string GetUserResponse { get; set; }

        public void OnGet(string accessToken, long expiresIn,string refreshToken,long refreshTokenExpiresIn,int refreshCount ,string tokenType, string getUserResponse)
        {
            Token = new TokenResponse
            {
                access_token = accessToken,
                expires_in = expiresIn,
                refresh_token = refreshToken,
                refresh_token_expires_in = refreshTokenExpiresIn,
                refresh_count = refreshCount,
                token_type = tokenType
            };
            GetUserResponse = getUserResponse;
        }

        public async Task<IActionResult> OnPostGetUserInfo()
        {
            var authSettings = new AuthSettingsViewModel();
            client.DefaultRequestHeaders.Add("cache-control", "no-cache");
            
            var request = new HttpRequestMessage(HttpMethod.Get, authSettings.UserInfoSeparateUri);

            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", Token.access_token);

            var response = await client.SendAsync(request);
            var responseString = await response.Content.ReadAsStringAsync();

            GetUserResponse = responseString;


            /*
             *{
    "nhsid_useruid": "910000000001",
    "name": "USERQ RANDOM Mr",
    "nhsid_nrbac_roles": [
        {
            "activities": [
                "Perform Prescription Preparation",
                "View Patient Medication",
                "Amend Patient Demographics",
                "Nurse Prescribers Formulary (NPF) Prescribing"
            ],
            "activity_codes": [
                "B0278",
                "B0401",
                "B0825",
                "B0058"
            ],
            "org_code": "RBA",
            "person_orgid": "555254239107",
            "person_roleid": "555254240100",
            "role_code": "S8000:G8000:R8001",
            "role_name": "\"Clinical\":\"Clinical Provision\":\"Nurse Access Role\""
        },
        {
            "activities": [
                "Manage Workgroups",
                "Independent Prescribing",
                "Execute CDS Extracts (NHS Group Pseud. Data)",
                "Manage Workgroup Membership"
            ],
            "activity_codes": [
                "B0100",
                "B0420",
                "B1510",
                "B0090"
            ],
            "org_code": "RBA",
            "person_orgid": "555254239107",
            "person_roleid": "555254242102",
            "role_code": "S8000:G8000:R8000",
            "role_name": "\"Clinical\":\"Clinical Provision\":\"Clinical Practitioner Access Role\""
        },
        {
            "activities": [
                "Perform Pharmacy Activities",
                "Verify Prescription",
                "Manage Pharmacy Activities",
                "View Patient Medication"
            ],
            "activity_codes": [
                "B0570",
                "B0068",
                "B0572",
                "B0401"
            ],
            "org_code": "RBA",
            "person_orgid": "555254239107",
            "person_roleid": "555254241101",
            "role_code": "S8000:G8000:R8003",
            "role_name": "\"Clinical\":\"Clinical Provision\":\"Health Professional Access Role\""
        },
        {
            "activities": [
                "Perform Pharmacy Activities",
                "Verify Prescription",
                "Manage Pharmacy Activities",
                "View Patient Medication"
            ],
            "activity_codes": [
                "B0570",
                "B0068",
                "B0572",
                "B0401"
            ],
            "org_code": "RBA",
            "person_orgid": "555254239107",
            "person_roleid": "093895563513",
            "role_code": "S8000:G8000:R8003",
            "role_name": "\"Clinical\":\"Clinical Provision\":\"Health Professional Access Role\""
        }
    ],
    "sub": "910000000001"
}
             *
             *
             */
            ModelState.Remove(nameof(GetUserResponse));
            return Page();
        }

        public async Task<IActionResult> OnPostRefreshToken()
        {
            //We now need to get the user info

            //We use the same token uri but different parameters
            var authSettings = new AuthSettingsViewModel();
            client.DefaultRequestHeaders.Add("cache-control", "no-cache");
            var dict = new Dictionary<string, string>()
            {
                {"grant_type", "refresh_token"},
                {"client_id", authSettings.CombinedApiKey},
                {"client_secret", authSettings.CombinedApiSecret},
                {"refresh_token", Token.refresh_token}
            };
            var postData = new FormUrlEncodedContent(dict);
            var request = new HttpRequestMessage(HttpMethod.Post, authSettings.TokenCombinedUri);
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
