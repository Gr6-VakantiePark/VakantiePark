using VakantiePark.Domain.Entities;

namespace VakantiePark.Domain.Interfaces
{
    public interface IParkSettingsRepository
    {
        Task<ParkSettings?> GetAsync();

        Task<List<OpeningHour>> GetOpeningHoursAsync();

        Task<List<PoolBasin>> GetPoolBasinsAsync();
    }
}
