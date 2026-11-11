using System.ComponentModel.DataAnnotations;

namespace VakantiePark.UI.Models;

public sealed class ContactMessage
{
    [Required(ErrorMessage = "Vul je naam in.")]
    public string Name { get; set; } = string.Empty;

    [Required(ErrorMessage = "Vul je e-mailadres in.")]
    [EmailAddress(ErrorMessage = "Vul een geldig e-mailadres in.")]
    public string Email { get; set; } = string.Empty;

    public string Subject { get; set; } = "Vraag over een boeking";

    public string? ReservationNumber { get; set; }

    [Required(ErrorMessage = "Schrijf je bericht.")]
    public string Message { get; set; } = string.Empty;
}
