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
    public class SeparateIntModel : PageModel
    {


        private readonly ILogger<SeparateModel> _logger;
        Random random = new Random();
        private string chars = "0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz";

        public SeparateIntModel(ILogger<SeparateModel> logger)
        {
            _logger = logger;
        }

        [BindProperty]
        public string HelperText => $"When authenticate is pressed we will redirect to {AuthSettings.AuthorizationRealSmartcardSeparateUri}?client_id={AuthSettings.SeparateApiKey}&redirect_uri={AuthSettings.RedirectSeparateUri}&response_type=code&state=random";


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
                {"client_id", AuthSettings.SeparateApiKey},
                {"redirect_uri", AuthSettings.RedirectSeparateUri },
                {"response_type", "code"},
                {"state", state}
            };
            var url = QueryHelpers.AddQueryString(AuthSettings.AuthorizationRealSmartcardSeparateUri, dictionary);

            return Redirect(url);
        }

    }
}
