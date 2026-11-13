using System.ComponentModel.DataAnnotations;
using VakantiePark.Shared.Common;

namespace VakantiePark.UI.Models;

public sealed class BookingDetails
{
    public const int NameMaxLength = 100;
    public const int EmailMaxLength = FieldLimits.EmailMaxLength;
    public const int PhoneMaxLength = 20;
    public const int StreetMaxLength = 100;
    public const int PostalCodeMaxLength = 10;
    public const int CityMaxLength = 100;

    [Required(ErrorMessage = "Vul je naam in.")]
    [StringLength(NameMaxLength, ErrorMessage = "Je naam is te lang.")]
    public string Name { get; set; } = string.Empty;

    [Required(ErrorMessage = "Vul je e-mailadres in.")]
    [EmailAddress(ErrorMessage = "Vul een geldig e-mailadres in.")]
    [StringLength(EmailMaxLength, ErrorMessage = "Je e-mailadres is te lang.")]
    public string Email { get; set; } = string.Empty;

    [Required(ErrorMessage = "Vul je telefoonnummer in.")]
    [Phone(ErrorMessage = "Vul een geldig telefoonnummer in.")]
    [StringLength(PhoneMaxLength, ErrorMessage = "Je telefoonnummer is te lang.")]
    public string Phone { get; set; } = string.Empty;

    [Required(ErrorMessage = "Vul je straat en huisnummer in.")]
    [StringLength(StreetMaxLength, ErrorMessage = "Je straat en huisnummer zijn te lang.")]
    public string Street { get; set; } = string.Empty;

    [Required(ErrorMessage = "Vul je postcode in.")]
    [StringLength(PostalCodeMaxLength, ErrorMessage = "Je postcode is te lang.")]
    public string PostalCode { get; set; } = string.Empty;

    [Required(ErrorMessage = "Vul je woonplaats in.")]
    [StringLength(CityMaxLength, ErrorMessage = "Je woonplaats is te lang.")]
    public string City { get; set; } = string.Empty;
}
