using System.ComponentModel.DataAnnotations;
using VakantiePark.Shared.Common;

namespace VakantiePark.UI.Models;

public sealed class LoginForm
{
    [Required(ErrorMessage = "Vul je e-mailadres in.")]
    [EmailAddress(ErrorMessage = "Vul een geldig e-mailadres in.")]
    [StringLength(FieldLimits.EmailMaxLength, ErrorMessage = "Je e-mailadres is te lang.")]
    public string Email { get; set; } = string.Empty;

    [Required(ErrorMessage = "Vul je wachtwoord in.")]
    [StringLength(FieldLimits.PasswordMaxLength, ErrorMessage = "Je wachtwoord is te lang.")]
    public string Password { get; set; } = string.Empty;
}
