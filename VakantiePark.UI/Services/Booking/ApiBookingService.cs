using System.Globalization;
using VakantiePark.Shared.Common;
using VakantiePark.Shared.DTOs.Accommodations;
using VakantiePark.Shared.DTOs.Park;
using VakantiePark.Shared.DTOs.Reservations;
using VakantiePark.UI.Interfaces;
using VakantiePark.UI.Models;

namespace VakantiePark.UI.Services
{
    public class ApiBookingService(IHttpService http, IParkService parkService) : IBookingService
    {
        private const string ParkInfoMissing = "De aankomst- en vertrektijden konden niet worden geladen.";

        public Task<ApiResponse<AccommodationDetailDto>> GetStayAsync(BookingRequest request) =>
            http.GetAsync<AccommodationDetailDto>(
                $"accommodations/{request.AccommodationId}" +
                $"?arrival={request.ArrivalDate.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture)}" +
                $"&departure={request.DepartureDate.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture)}" +
                $"&persons={request.Persons}" +
                $"&youngChildren={request.YoungChildren}" +
                $"&pets={request.Pets}");

        public async Task<ApiResponse<bool>> CheckAvailabilityAsync(BookingRequest request)
        {
            var park = await GetParkInfoAsync();
            if (park == null)
            {
                return ApiResponse<bool>.Fail(ParkInfoMissing);
            }

            return await http.GetAsync<bool>(
                "accommodations/availability" +
                $"?start={ToQueryValue(ArrivalOn(request.ArrivalDate, park))}" +
                $"&end={ToQueryValue(DepartureOn(request.DepartureDate, park))}" +
                $"&accommodationId={request.AccommodationId}" +
                $"&amountOfPeople={request.Persons}");
        }

        public async Task<ApiResponse<BookingDto>> CreateAsync(BookingRequest request, GuestDto guest, decimal expectedTotalPrice)
        {
            var park = await GetParkInfoAsync();
            if (park == null)
            {
                return ApiResponse<BookingDto>.Fail(ParkInfoMissing);
            }

            return await http.PostAsync<BookingDto, CreateBookingDto>("boekingen", new CreateBookingDto
            {
                AccommodationId = request.AccommodationId,
                StartDate = ArrivalOn(request.ArrivalDate, park),
                EndDate = DepartureOn(request.DepartureDate, park),
                NumberOfGuests = request.Persons,
                YoungChildren = request.YoungChildren,
                Pets = request.Pets,
                ExpectedTotalPrice = expectedTotalPrice,
                HasPaid = false,
                Guest = guest,
            });
        }

        private async Task<ParkInfoDto?> GetParkInfoAsync()
        {
            var response = await parkService.GetInfoAsync();
            return response.Succeeded ? response.Value : null;
        }

        private static DateTime ArrivalOn(DateOnly date, ParkInfoDto park) => date.ToDateTime(ParseTime(park.ArrivalTime));

        private static DateTime DepartureOn(DateOnly date, ParkInfoDto park) => date.ToDateTime(ParseTime(park.DepartureTime));

        private static TimeOnly ParseTime(string time) => TimeOnly.ParseExact(time, "HH:mm", CultureInfo.InvariantCulture);

        private static string ToQueryValue(DateTime value) =>
            Uri.EscapeDataString(value.ToString("yyyy-MM-ddTHH:mm:ss", CultureInfo.InvariantCulture));
    }
}
