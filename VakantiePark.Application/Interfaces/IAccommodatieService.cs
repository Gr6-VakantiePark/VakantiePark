using VakantiePark.Shared.DTOs.Accommodaties;

namespace VakantiePark.Application.Interfaces
{
    public interface IAccommodatieService
    {
        Task<List<AccommodatieDto>> GetAllAsync();

        /// <exception cref="KeyNotFoundException">Als de accommodatie niet bestaat.</exception>
        Task<AccommodatieDetailDto> GetByIdAsync(int id);
    }
}
