using System.ComponentModel.DataAnnotations;
using VakantiePark.Shared.Common;

namespace VakantiePark.UI.Models;

public sealed class ContactMessage
{
    public const int NameMaxLength = 100;
    public const int EmailMaxLength = FieldLimits.EmailMaxLength;
    public const int ReservationNumberMaxLength = 20;
    public const int MessageMaxLength = 2000;

    [Required(ErrorMessage = "Vul je naam in.")]
    [StringLength(NameMaxLength, ErrorMessage = "Je naam is te lang.")]
    public string Name { get; set; } = string.Empty;

    [Required(ErrorMessage = "Vul je e-mailadres in.")]
    [EmailAddress(ErrorMessage = "Vul een geldig e-mailadres in.")]
    [StringLength(EmailMaxLength, ErrorMessage = "Je e-mailadres is te lang.")]
    public string Email { get; set; } = string.Empty;

    public string Subject { get; set; } = "Vraag over een boeking";

    [StringLength(ReservationNumberMaxLength, ErrorMessage = "Het reserveringsnummer is te lang.")]
    public string? ReservationNumber { get; set; }

    [Required(ErrorMessage = "Schrijf je bericht.")]
    [StringLength(MessageMaxLength, ErrorMessage = "Je bericht is te lang.")]
    public string Message { get; set; } = string.Empty;
}
