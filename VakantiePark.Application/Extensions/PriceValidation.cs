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
            if (request.Arrival == default || request.Departure == default)
                throw new ArgumentException("Vul een aankomst- en vertrekdatum in.");
            if (request.Departure <= request.Arrival)
                throw new ArgumentException("De vertrekdatum moet na de aankomstdatum liggen.");
            if (request.Persons < 1)
                throw new ArgumentException("Er moet minimaal 1 persoon meegaan.");
        }
    }
}
