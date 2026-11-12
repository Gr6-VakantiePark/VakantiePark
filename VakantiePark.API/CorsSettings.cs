using System.ComponentModel.DataAnnotations;

namespace VakantiePark.API
{
    public class CorsSettings
    {
        public const string SectionName = "Cors";
        public const string PolicyName = "frontend";

        [Required(ErrorMessage = "Cors:AllowedOrigins ontbreekt in de configuratie.")]
        [MinLength(1, ErrorMessage = "Geef minimaal één toegestane origin op in Cors:AllowedOrigins.")]
        public string[] AllowedOrigins { get; set; } = [];
    }
}
