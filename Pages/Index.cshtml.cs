using CIS2Auth.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CIS2Auth.Pages
{
    public class IndexModel : PageModel
    {

        private readonly ILogger<IndexModel> _logger;
        Random random = new Random();
        private string chars = "0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz";

        public IndexModel(ILogger<IndexModel> logger)
        {
            _logger = logger;
        }


        [BindProperty]
        public string HelperText => $"When authenticate is pressed we will redirect to {AuthSettings.AuthorizationCombinedUri}?clientid={AuthSettings.CombinedApiKey}&redirecturi={AuthSettings.RedirectCombinedUri}&response_type=code&state=random";


        [BindProperty]
        public AuthSettingsViewModel AuthSettings { get; set; }

        public void OnGet()
        {
            AuthSettings = new AuthSettingsViewModel();
        }

        public IActionResult OnPost()
        {
            var state = new string(Enumerable.Repeat(chars, 16).Select(s => s[random.Next(s.Length)]).ToArray());
            var dictionary = new Dictionary<string, string>()
            {
                {"client_id", AuthSettings.CombinedApiKey},
                {"redirect_uri", AuthSettings.RedirectCombinedUri },
                {"response_type", "code"},
                {"state", state}
            };
            var url = QueryHelpers.AddQueryString(AuthSettings.AuthorizationCombinedUri, dictionary);

            return Redirect(url);

            //This will do a redirection to https://sandbox.api.service.nhs.uk/oauth2/authorize?clientid=clientid&redirecturi=d&response_type=code&state=random

            //return RedirectToPage("Callback", new {code = "test", state = "state"});
        }

    }
}
