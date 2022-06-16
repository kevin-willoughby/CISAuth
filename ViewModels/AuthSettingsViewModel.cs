using System.ComponentModel.DataAnnotations;

namespace CIS2Auth.ViewModels
{
    public class AuthSettingsViewModel
    {
        [Required]
        public string CombinedApiKey { get; set; } = "hxPNYXv0nFjYPtvKYl8GyUGH4oRKs18N";
        [Required]
        public string CombinedApiSecret { get; set; } = "Ti4Y7lUBRY2ZOZEm";


        [Required]
        public string SeparateApiKey { get; set; } = "ndeVkO78umbLNdF9GRvLJ9UuLnE18Jf1";
        [Required]
        public string SeparateApiSecret { get; set; } = "33rJ1MManKhO8KeG";

        private const string SandpitSeparateBase = "https://int.api.service.nhs.uk/oauth2-no-smartcard/";
        //private const string SandpitBase = "https://sandbox.api.service.nhs.uk/oauth2/";
        private const string SandpitCombinedBase = "https://sandbox.api.service.nhs.uk/oauth2/";

        //
        [Required]
        public string AuthorizationSeparateUri { get; set; } = SandpitSeparateBase + "authorize";
        [Required]
        public string TokenSeparateUri { get; set; } = SandpitSeparateBase + "token";
        [Required]
        public string UserInfoSeparateUri { get; set; } = "https://int.api.service.nhs.uk/oauth2/userinfo";
        [Required]
        public string RedirectSeparateUri { get; set; } = "https://hdsdev.resip.co.uk/CIS2/SeparateCallback";


        [Required]
        public string AuthorizationCombinedUri { get; set; } = SandpitCombinedBase + "authorize";
        [Required]
        public string TokenCombinedUri { get; set; } = SandpitCombinedBase + "token";
        [Required]
        public string UserInfoCombinedUri { get; set; } = SandpitCombinedBase + "userinfo";
        [Required]
        public string RedirectCombinedUri { get; set; } = "https://hdsdev.resip.co.uk/CIS2/Callback";


    }
}
