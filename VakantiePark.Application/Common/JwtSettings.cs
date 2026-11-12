using System.ComponentModel.DataAnnotations;

namespace VakantiePark.Application.Common
{
    public class JwtSettings
    {
        public const string SectionName = "JWT";
        public const int MinimumKeyLength = 32;

        private const string KeyMissing =
            "JWT:Key ontbreekt of is korter dan 32 tekens. Zet de sleutel in user-secrets "
            + "(dotnet user-secrets set \"JWT:Key\" \"<sleutel>\" --project VakantiePark.API) "
            + "of in de omgevingsvariabele JWT__Key.";

        [Required(ErrorMessage = "JWT:Issuer ontbreekt in de configuratie.")]
        public string Issuer { get; set; } = string.Empty;
        public string? Audience { get; set; }

        [Required(ErrorMessage = KeyMissing)]
        [MinLength(MinimumKeyLength, ErrorMessage = KeyMissing)]
        public string Key { get; set; } = string.Empty;

        [Range(1, 1440, ErrorMessage = "JWT:ExpiryMinutes moet tussen 1 en 1440 liggen.")]
        public int ExpiryMinutes { get; set; } = 60;

        public string EffectiveAudience => string.IsNullOrWhiteSpace(Audience) ? Issuer : Audience;
    }
}
