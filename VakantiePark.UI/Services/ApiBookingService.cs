using System.Globalization;
using VakantiePark.Shared.Common;
using VakantiePark.Shared.DTOs.Accommodations;
using VakantiePark.UI.Interfaces;
using VakantiePark.UI.Models;

namespace VakantiePark.UI.Services
{
    public class ApiBookingService(IHttpService http) : IBookingService
    {
        public Task<ApiResponse<AccommodationDetailDto>> GetStayAsync(BookingRequest request) =>
            http.GetAsync<AccommodationDetailDto>(
                $"accommodations/{request.AccommodationId}" +
                $"?arrival={request.ArrivalDate.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture)}" +
                $"&departure={request.DepartureDate.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture)}" +
                $"&persons={request.Persons}");
    }
}
