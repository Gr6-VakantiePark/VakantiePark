using System.Globalization;
using VakantiePark.Shared.Common;
using VakantiePark.Shared.DTOs.Accommodations;
using VakantiePark.Shared.DTOs.Reservations;
using VakantiePark.UI.Data;
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
                $"&persons={request.Persons}" +
                $"&youngChildren={request.YoungChildren}" +
                $"&pets={request.Pets}");

        public Task<ApiResponse<bool>> CheckAvailabilityAsync(BookingRequest request) =>
            http.GetAsync<bool>(
                "accommodations/availability" +
                $"?start={ToQueryValue(BookingRules.ArrivalOn(request.ArrivalDate))}" +
                $"&end={ToQueryValue(BookingRules.DepartureOn(request.DepartureDate))}" +
                $"&accommodationId={request.AccommodationId}" +
                $"&amountOfPeople={request.Persons}");

        public Task<ApiResponse<BookingDto>> CreateAsync(BookingRequest request, GuestDto guest, decimal expectedTotalPrice) =>
            http.PostAsync<BookingDto, CreateBookingDto>("boekingen", new CreateBookingDto
            {
                AccommodationId = request.AccommodationId,
                StartDate = BookingRules.ArrivalOn(request.ArrivalDate),
                EndDate = BookingRules.DepartureOn(request.DepartureDate),
                NumberOfGuests = request.Persons,
                YoungChildren = request.YoungChildren,
                Pets = request.Pets,
                ExpectedTotalPrice = expectedTotalPrice,
                HasPaid = false,
                Guest = guest,
            });

        private static string ToQueryValue(DateTime value) =>
            Uri.EscapeDataString(value.ToString("yyyy-MM-ddTHH:mm:ss", CultureInfo.InvariantCulture));
    }
}
