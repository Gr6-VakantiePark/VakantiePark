namespace VakantiePark.UI.Models;

public sealed record BookingRequest(
    Guid AccommodationId,
    DateOnly ArrivalDate,
    DateOnly DepartureDate,
    int Persons,
    int Pets)
{
    public int Nights => DepartureDate.DayNumber - ArrivalDate.DayNumber;
}
