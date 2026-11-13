using System.ComponentModel.DataAnnotations;
using VakantiePark.Shared.Common;
using VakantiePark.Shared.DTOs.Reservations;

namespace VakantiePark.UI.Models;

public sealed class BookingDetails
{
    public const int NameMaxLength = FieldLimits.GuestNameMaxLength;
    public const int EmailMaxLength = FieldLimits.EmailMaxLength;
    public const int PhoneMaxLength = FieldLimits.PhoneNumberMaxLength;

    [Required(ErrorMessage = "Vul je naam in.")]
    [StringLength(NameMaxLength, ErrorMessage = "Je naam is te lang.")]
    public string Name { get; set; } = string.Empty;

    [Required(ErrorMessage = "Vul je e-mailadres in.")]
    [EmailAddress(ErrorMessage = "Vul een geldig e-mailadres in.")]
    [StringLength(EmailMaxLength, ErrorMessage = "Je e-mailadres is te lang.")]
    public string Email { get; set; } = string.Empty;

    [RegularExpression(@"^\+?[0-9\s\-()]*$", ErrorMessage = "Vul een geldig telefoonnummer in.")]
    [StringLength(PhoneMaxLength, ErrorMessage = "Je telefoonnummer is te lang.")]
    public string? Phone { get; set; }

    public GuestDto ToGuestDto() => new()
    {
        Name = Name.Trim(),
        Email = Email.Trim(),
        PhoneNumber = string.IsNullOrWhiteSpace(Phone) ? null : Phone.Trim(),
    };
}
