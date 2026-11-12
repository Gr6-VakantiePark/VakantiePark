using VakantiePark.Shared.DTOs.Prices;

namespace VakantiePark.Application.Interfaces
{
    public interface IPriceService
    {
        Task<PriceDto> CalculateAsync(PriceRequestDto request);
    }
}
