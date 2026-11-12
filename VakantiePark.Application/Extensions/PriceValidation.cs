using VakantiePark.Application.Extensions.Accommodations;
using VakantiePark.Shared.Common;
using VakantiePark.Shared.DTOs.Prices;

namespace VakantiePark.Application.Extensions
{
    public static class PriceValidation
    {
        public static void Validate(this PriceRequestDto? request)
        {
            if (request is null)
                throw new ArgumentException("De prijsaanvraag ontbreekt.");
            if (string.IsNullOrWhiteSpace(request.Type))
                throw new ArgumentException("Kies een type accommodatie.");
            if (request.Type.Trim().Length > FieldLimits.SlugMaxLength)
                throw new ArgumentException($"Het type mag maximaal {FieldLimits.SlugMaxLength} tekens hebben.");

            AccommodationValidation.ValidatePeriod(request.Arrival, request.Departure);
            AccommodationValidation.ValidatePersons(request.Persons);
        }
    }
}
