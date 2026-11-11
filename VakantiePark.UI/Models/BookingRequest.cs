using VakantiePark.UI.Data;

namespace VakantiePark.UI.Models;

public sealed record BookingRequest(
    AccommodationType Type,
    DateOnly ArrivalDate,
    DateOnly DepartureDate,
    int Adults,
    int Children,
    int Pets)
{
    public int Persons => Adults + Children;

    public int Nights => DepartureDate.DayNumber - ArrivalDate.DayNumber;
}
