using System.Globalization;

namespace VakantiePark.UI.Data;

public static class BookingRules
{
    public const string ArrivalTime = "15:00";
    public const string DepartureTime = "10:00";

    public static DateTime ArrivalOn(DateOnly date) => date.ToDateTime(ParseTime(ArrivalTime));

    public static DateTime DepartureOn(DateOnly date) => date.ToDateTime(ParseTime(DepartureTime));

    private static TimeOnly ParseTime(string time) => TimeOnly.ParseExact(time, "HH:mm", CultureInfo.InvariantCulture);
}
