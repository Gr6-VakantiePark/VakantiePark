using System.Globalization;
using VakantiePark.Shared.Common;
using VakantiePark.Shared.DTOs.Prices;
using VakantiePark.UI.Interfaces;
using VakantiePark.UI.Models;

namespace VakantiePark.UI.Services
{
    public class PriceService(IHttpService httpService) : IPriceService
    {
        public Task<ApiResponse<PriceDto>> GetPriceAsync(BookingRequest request) =>
            httpService.GetAsync<PriceDto>(
                $"accommodaties/price?type={Uri.EscapeDataString(request.Type.Slug)}" +
                $"&arrival={request.ArrivalDate.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture)}" +
                $"&departure={request.DepartureDate.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture)}" +
                $"&persons={request.Persons}");
    }
}
