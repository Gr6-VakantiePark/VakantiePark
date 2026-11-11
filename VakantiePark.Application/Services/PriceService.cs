using VakantiePark.Application.Calculators;
using VakantiePark.Application.Extensions;
using VakantiePark.Application.Interfaces;
using VakantiePark.Shared.DTOs.Prices;

namespace VakantiePark.Application.Services
{
    public class PriceService(IAccommodationRepository accommodationRepository, IParkSettingsRepository settingsRepository) : IPriceService
    {
        public async Task<PriceDto> CalculateAsync(PriceRequestDto request)
        {
            request.Validate();

            var type = await accommodationRepository.GetTypeBySlugAsync(request.Type.Trim())
                ?? throw new KeyNotFoundException($"Accommodatietype '{request.Type}' is niet gevonden.");

            int maxPersons = type.Accommodations.Count == 0 ? 0 : type.Accommodations.Max(a => a.MaxPersons);
            if (request.Persons > maxPersons)
                throw new ArgumentException($"{type.Name} is geschikt voor maximaal {maxPersons} personen.");

            var settings = await settingsRepository.GetAsync()
                ?? throw new KeyNotFoundException("De parkinstellingen zijn niet gevonden.");

            int nights = request.Departure.DayNumber - request.Arrival.DayNumber;

            return PriceCalculator.Calculate(
                type.PricePerNight,
                nights,
                request.Persons,
                settings.CleaningFee,
                settings.TouristTaxPerPersonPerNight);
        }
    }
}
