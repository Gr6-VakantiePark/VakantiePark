using VakantiePark.Application.Interfaces;
using VakantiePark.Application.Extensions;
using VakantiePark.Shared.DTOs.Accommodaties;

namespace VakantiePark.Application.Services
{
    public class AccommodatieService(IAccommodatieRepository repository) : IAccommodatieService
    {
        public async Task<List<AccommodatieDto>> GetAllAsync()
        {
            var accommodaties = await repository.GetAllAsync();
            return accommodaties.Select(a => a.ToDto()).ToList();
        }

        public async Task<AccommodatieDetailDto> GetByIdAsync(int id)
        {
            var accommodatie = await repository.GetByIdAsync(id)
                ?? throw new KeyNotFoundException($"Accommodatie met id {id} is niet gevonden.");

            return accommodatie.ToDetailDto();
        }
    }
}
