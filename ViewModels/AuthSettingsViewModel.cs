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

        [Required]
        public string AppAccessApiKey { get; set; } = "ndeVkO78umbLNdF9GRvLJ9UuLnE18Jf1";

        private const string SandpitSeparateBase = "https://int.api.service.nhs.uk/oauth2-no-smartcard/";
        //private const string SandpitBase = "https://sandbox.api.service.nhs.uk/oauth2/";
        private const string SandpitCombinedBase = "https://sandbox.api.service.nhs.uk/oauth2/";

        private const string SandpitAppAccessBase = "https://int.api.service.nhs.uk/oauth2/";

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

        [Required]
        public string TokenApplicationAccessUri { get; set; } = SandpitAppAccessBase + "token";


        [Required]
        public string PUBLIC_KEY = @"-----BEGIN PUBLIC KEY-----
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

        [Required]
        public string PRIVATE_KEY = @"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";

        [Required]
        public string Kid = "dev-1";

    }
}
