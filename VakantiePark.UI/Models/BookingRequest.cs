namespace VakantiePark.UI.Models;

public sealed record BookingRequest(
    int AccommodationId,
    DateOnly ArrivalDate,
    DateOnly DepartureDate,
    int Persons,
    int Pets)
{
    public int Nights => DepartureDate.DayNumber - ArrivalDate.DayNumber;
}
