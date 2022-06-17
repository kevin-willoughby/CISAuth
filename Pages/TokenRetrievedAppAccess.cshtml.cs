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
    public class TokenRetrievedAppAccessModel : PageModel
    {

        [BindProperty]
        public TokenResponse Token { get; set; }


        [BindProperty]
        public string GetUserResponse { get; set; }

        public void OnGet(string accessToken, long expiresIn,string tokenType, string getUserResponse)
        {
            Token = new TokenResponse
            {
                access_token = accessToken,
                expires_in = expiresIn,
                token_type = tokenType
            };
            GetUserResponse = getUserResponse;
        }

        
    }
}
