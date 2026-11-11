using VakantiePark.Domain.Entities;

namespace VakantiePark.Application.Interfaces
{
    public interface IParkSettingsRepository
    {
        Task<ParkSettings?> GetAsync();
    }
}
