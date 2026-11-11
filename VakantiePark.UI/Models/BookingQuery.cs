using System.Globalization;
using VakantiePark.UI.Data;

namespace VakantiePark.UI.Models;

public static class BookingQuery
{
    public const string Type = "type";
    public const string Arrival = "aankomst";
    public const string Departure = "vertrek";
    public const string Adults = "volwassenen";
    public const string Children = "kinderen";
    public const string Pets = "huisdieren";

    private const string DateFormat = "yyyy-MM-dd";

    public static bool TryParse(
        string? typeSlug,
        string? arrival,
        string? departure,
        string? adults,
        string? children,
        string? pets,
        DateOnly today,
        out BookingRequest? request,
        out string error)
    {
        request = null;

        var type = string.IsNullOrWhiteSpace(typeSlug) ? null : ParkData.GetBySlug(typeSlug);
        if (type is null)
        {
            error = "Kies eerst een verblijf.";
            return false;
        }

        if (!TryParseDate(arrival, out var arrivalDate) || !TryParseDate(departure, out var departureDate))
        {
            error = "Kies eerst je aankomst- en vertrekdatum.";
            return false;
        }

        if (arrivalDate < today)
        {
            error = "Je aankomstdatum ligt in het verleden.";
            return false;
        }

        if (departureDate <= arrivalDate)
        {
            error = "Je vertrekdatum moet na je aankomstdatum liggen.";
            return false;
        }

        if (!TryParseCount(adults, out var adultCount) || adultCount < 1)
        {
            error = "Kies met hoeveel volwassenen je komt.";
            return false;
        }

        var childCount = 0;
        var petCount = 0;
        if (!string.IsNullOrEmpty(children) && !TryParseCount(children, out childCount)
            || !string.IsNullOrEmpty(pets) && !TryParseCount(pets, out petCount))
        {
            error = "Je reisgezelschap is niet goed ingevuld.";
            return false;
        }

        var maxPersons = BookingRules.MaxPersons(type);
        if (adultCount + childCount > maxPersons)
        {
            error = $"{type.Name} is voor maximaal {maxPersons} personen.";
            return false;
        }

        if (petCount > BookingRules.MaxPets)
        {
            error = $"Je kunt maximaal {BookingRules.MaxPets} huisdieren meenemen.";
            return false;
        }

        request = new BookingRequest(type, arrivalDate, departureDate, adultCount, childCount, petCount);
        error = string.Empty;
        return true;
    }

    private static bool TryParseDate(string? value, out DateOnly date) =>
        DateOnly.TryParseExact(value, DateFormat, CultureInfo.InvariantCulture, DateTimeStyles.None, out date);

    private static bool TryParseCount(string? value, out int count) =>
        int.TryParse(value, NumberStyles.None, CultureInfo.InvariantCulture, out count);
}
