using System.ComponentModel.DataAnnotations;
using VakantiePark.Shared.Common;

namespace VakantiePark.UI.Models;

public sealed class EmployeeForm
{
    [Required(ErrorMessage = "Vul een voornaam in.")]
    [StringLength(FieldLimits.FirstNameMaxLength, ErrorMessage = "De voornaam is te lang.")]
    public string FirstName { get; set; } = string.Empty;

    [Required(ErrorMessage = "Vul een achternaam in.")]
    [StringLength(FieldLimits.LastNameMaxLength, ErrorMessage = "De achternaam is te lang.")]
    public string LastName { get; set; } = string.Empty;

    [Required(ErrorMessage = "Vul een e-mailadres in.")]
    [EmailAddress(ErrorMessage = "Vul een geldig e-mailadres in.")]
    [StringLength(FieldLimits.EmailMaxLength, ErrorMessage = "Het e-mailadres is te lang.")]
    public string Email { get; set; } = string.Empty;
}
