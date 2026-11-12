using VakantiePark.Application.Calculators;
using VakantiePark.Application.Extensions;
using VakantiePark.Application.Interfaces;
using VakantiePark.Domain.Interfaces;
using VakantiePark.Shared.DTOs.Prices;

namespace VakantiePark.Application.Services.Prices
{
    public class PriceService(IAccommodationRepository accommodationRepository, IParkSettingsRepository settingsRepository) : IPriceService
    {
        public async Task<PriceDto> CalculateAsync(PriceRequestDto request)
        {
            request.Validate();

            var type = await accommodationRepository.GetTypeBySlugAsync(request.Type.Trim())
                ?? throw new KeyNotFoundException($"Accommodatietype '{request.Type}' is niet gevonden.");

            var maxPersons = type.Accommodations.Count == 0 ? 0 : type.Accommodations.Max(a => a.MaxPersons);
            if (request.Persons > maxPersons)
            {
                throw new ArgumentException($"{type.Name} is geschikt voor maximaal {maxPersons} personen.");
            }

            var maxPets = type.Accommodations.Count == 0 ? 0 : type.Accommodations.Max(a => a.MaxPets);
            if (request.Pets > maxPets)
            {
                throw new ArgumentException($"In {type.Name} mogen maximaal {maxPets} huisdieren mee.");
            }

            var settings = await settingsRepository.GetAsync()
                ?? throw new KeyNotFoundException("De parkinstellingen zijn niet gevonden.");

            return PriceCalculator.Calculate(
                type,
                request.Arrival,
                request.Departure,
                request.Persons,
                request.YoungChildren,
                request.Pets,
                settings);
        }
    }
}
