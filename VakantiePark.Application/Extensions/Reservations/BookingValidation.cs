using VakantiePark.Shared.Common;
using VakantiePark.Shared.DTOs.Reservations;

namespace VakantiePark.Application.Extensions
{
    public static class BookingValidation
    {
        public static void Validate(this CreateBookingDto? request)
        {
            if (request is null)
                throw new ArgumentException("De gegevens van de boeking ontbreken.");
            if (request.Guest is null)
                throw new ArgumentException("De gegevens van de gast ontbreken.");

            if (string.IsNullOrWhiteSpace(request.Guest.Name))
                throw new ArgumentException("Vul je naam in.");
            if (request.Guest.Name.Trim().Length > FieldLimits.GuestNameMaxLength)
                throw new ArgumentException($"Je naam mag maximaal {FieldLimits.GuestNameMaxLength} tekens hebben.");

            string email = EmployeeMappings.NormalizeEmail(request.Guest.Email ?? string.Empty);
            if (email.Length > FieldLimits.EmailMaxLength)
                throw new ArgumentException($"Het e-mailadres mag maximaal {FieldLimits.EmailMaxLength} tekens hebben.");
            if (!AuthValidation.IsValidEmail(email))
                throw new ArgumentException("Vul een geldig e-mailadres in.");

            if (request.Guest.PhoneNumber is { } phone && phone.Trim().Length > FieldLimits.PhoneNumberMaxLength)
                throw new ArgumentException($"Het telefoonnummer mag maximaal {FieldLimits.PhoneNumberMaxLength} tekens hebben.");

            if (request.ExpectedTotalPrice <= 0m)
                throw new ArgumentException("De prijs van de boeking ontbreekt.");
        }
    }
}
