namespace VakantiePark.Application.Common
{
    public class JwtSettings
    {
        public const string SectionName = "JWT";

        public string Issuer { get; set; } = string.Empty;
        public string? Audience { get; set; }
        public string Key { get; set; } = string.Empty;
        public int ExpiryMinutes { get; set; } = 60;

        public string EffectiveAudience => string.IsNullOrWhiteSpace(Audience) ? Issuer : Audience;
    }
}
