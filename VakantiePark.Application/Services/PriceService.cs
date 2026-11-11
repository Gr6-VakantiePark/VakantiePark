using VakantiePark.Application.Calculators;
using VakantiePark.Application.Extensions;
using VakantiePark.Application.Interfaces;
using VakantiePark.Shared.DTOs.Prices;

namespace VakantiePark.Application.Services
{
    public class PriceService(IAccommodatieRepository accommodationRepository, IParkSettingsRepository settingsRepository) : IPriceService
    {
        public async Task<PriceDto> CalculateAsync(PriceRequestDto request)
        {
            request.Validate();

            var type = await accommodationRepository.GetTypeBySlugAsync(request.Type.Trim())
                ?? throw new KeyNotFoundException($"Accommodatietype '{request.Type}' is niet gevonden.");

            int maxPersons = type.Accommodaties.Count == 0 ? 0 : type.Accommodaties.Max(a => a.MaxPersonen);
            if (request.Persons > maxPersons)
                throw new ArgumentException($"{type.Naam} is geschikt voor maximaal {maxPersons} personen.");

            var settings = await settingsRepository.GetAsync()
                ?? throw new KeyNotFoundException("De parkinstellingen zijn niet gevonden.");

            int nights = request.Departure.DayNumber - request.Arrival.DayNumber;

            return PriceCalculator.Calculate(
                type.PrijsPerNacht,
                nights,
                request.Persons,
                settings.CleaningFee,
                settings.TouristTaxPerPersonPerNight);
        }
    }
}
