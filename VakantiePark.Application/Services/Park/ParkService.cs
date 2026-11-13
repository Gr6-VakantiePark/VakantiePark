using VakantiePark.Application.Extensions.Park;
using VakantiePark.Application.Interfaces;
using VakantiePark.Domain.Interfaces;
using VakantiePark.Shared.DTOs.Park;

namespace VakantiePark.Application.Services
{
    public class ParkService(IParkSettingsRepository settingsRepository, IAccommodationRepository accommodationRepository) : IParkService
    {
        public const string NearPoolAmenityCode = "nearPool";

        public async Task<ParkInfoDto> GetInfoAsync()
        {
            var settings = await settingsRepository.GetAsync();
            if (settings == null)
            {
                throw new KeyNotFoundException("De parkinstellingen zijn niet gevonden.");
            }

            var openingHours = await settingsRepository.GetOpeningHoursAsync();
            var poolBasins = await settingsRepository.GetPoolBasinsAsync();
            var accommodationsNearPool = await accommodationRepository.CountWithAmenityAsync(NearPoolAmenityCode);

            return settings.ToInfoDto(openingHours, poolBasins, accommodationsNearPool);
        }
    }
}
