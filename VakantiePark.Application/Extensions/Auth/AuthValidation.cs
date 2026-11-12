using System.Net.Mail;
using VakantiePark.Shared.Common;
using VakantiePark.Shared.DTOs.Auth;

namespace VakantiePark.Application.Extensions
{
    public static class AuthValidation
    {
        public static void Validate(this LoginRequestDto? request)
        {
            if (request is null)
                throw new ArgumentException("De inloggegevens ontbreken.");
            if (string.IsNullOrWhiteSpace(request.Email) || string.IsNullOrEmpty(request.Password))
                throw new ArgumentException("Vul je e-mailadres en wachtwoord in.");
            if (request.Email.Trim().Length > FieldLimits.EmailMaxLength)
                throw new ArgumentException($"Het e-mailadres mag maximaal {FieldLimits.EmailMaxLength} tekens hebben.");
            if (request.Password.Length > FieldLimits.PasswordMaxLength)
                throw new ArgumentException($"Het wachtwoord mag maximaal {FieldLimits.PasswordMaxLength} tekens hebben.");
        }

        public static void Validate(this CreateEmployeeDto? request)
        {
            if (request is null)
                throw new ArgumentException("De gegevens van de medewerker ontbreken.");

            string email = EmployeeMappings.NormalizeEmail(request.Email ?? string.Empty);
            if (email.Length > FieldLimits.EmailMaxLength)
                throw new ArgumentException($"Het e-mailadres mag maximaal {FieldLimits.EmailMaxLength} tekens hebben.");
            if (!IsValidEmail(email))
                throw new ArgumentException("Vul een geldig e-mailadres in.");

            if (string.IsNullOrWhiteSpace(request.FirstName) || string.IsNullOrWhiteSpace(request.LastName))
                throw new ArgumentException("Vul een voor- en achternaam in.");
            if (request.FirstName.Trim().Length > FieldLimits.FirstNameMaxLength)
                throw new ArgumentException($"De voornaam mag maximaal {FieldLimits.FirstNameMaxLength} tekens hebben.");
            if (request.LastName.Trim().Length > FieldLimits.LastNameMaxLength)
                throw new ArgumentException($"De achternaam mag maximaal {FieldLimits.LastNameMaxLength} tekens hebben.");

            if (string.IsNullOrEmpty(request.Password) || request.Password.Length < FieldLimits.PasswordMinLength)
                throw new ArgumentException($"Het wachtwoord moet minimaal {FieldLimits.PasswordMinLength} tekens hebben.");
            if (request.Password.Length > FieldLimits.PasswordMaxLength)
                throw new ArgumentException($"Het wachtwoord mag maximaal {FieldLimits.PasswordMaxLength} tekens hebben.");

            if (request.Roles is null || request.Roles.Count == 0)
                throw new ArgumentException("Kies minimaal één rol.");
            if (request.Roles.Any(role => string.IsNullOrWhiteSpace(role) || role.Trim().Length > FieldLimits.RoleCodeMaxLength))
                throw new ArgumentException("Een of meer rollen zijn ongeldig.");
        }

        private static bool IsValidEmail(string email) =>
            MailAddress.TryCreate(email, out var address) && address.Address == email;
    }
}
