using System.Globalization;
using VakantiePark.Shared.DTOs.Accommodations;

namespace VakantiePark.UI.Models;

public static class BookingQuery
{
    public const string Page = "boeken";
    public const string Accommodation = "accommodatie";
    public const string Arrival = "aankomst";
    public const string Departure = "vertrek";
    public const string Persons = "personen";
    public const string Pets = "huisdieren";

    private const string DateFormat = "yyyy-MM-dd";

    public static string For(Guid accommodationId, AccommodationSearchCriteria criteria) =>
        $"{Page}?{Accommodation}={accommodationId}" +
        $"&{Arrival}={criteria.Arrival.ToString(DateFormat, CultureInfo.InvariantCulture)}" +
        $"&{Departure}={criteria.Departure.ToString(DateFormat, CultureInfo.InvariantCulture)}" +
        $"&{Persons}={criteria.Persons}" +
        (criteria.Pets > 0 ? $"&{Pets}={criteria.Pets}" : string.Empty);

    public static bool TryParse(
        string? accommodation,
        string? arrival,
        string? departure,
        string? persons,
        string? pets,
        DateOnly today,
        out BookingRequest? request,
        out string error)
    {
        request = null;

        if (!Guid.TryParse(accommodation, out var accommodationId) || accommodationId == Guid.Empty)
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

        if (!TryParseCount(persons, out var personCount) || personCount < 1)
        {
            error = "Kies met hoeveel personen je komt.";
            return false;
        }

        var petCount = 0;
        if (!string.IsNullOrEmpty(pets) && !TryParseCount(pets, out petCount))
        {
            error = "Het aantal huisdieren is niet goed ingevuld.";
            return false;
        }

        request = new BookingRequest(accommodationId, arrivalDate, departureDate, personCount, petCount);
        error = string.Empty;
        return true;
    }

    public static string? CheckAgainst(BookingRequest request, AccommodationDetailDto accommodation)
    {
        if (request.Persons > accommodation.MaxPersons)
            return $"{accommodation.Name} is voor maximaal {accommodation.MaxPersons} personen.";

        if (request.Pets > accommodation.MaxPets)
            return accommodation.MaxPets == 0
                ? $"In {accommodation.Name} zijn geen huisdieren toegestaan."
                : $"In {accommodation.Name} mag je maximaal {accommodation.MaxPets} huisdieren meenemen.";

        return null;
    }

    private static bool TryParseDate(string? value, out DateOnly date) =>
        DateOnly.TryParseExact(value, DateFormat, CultureInfo.InvariantCulture, DateTimeStyles.None, out date);

    private static bool TryParseCount(string? value, out int count) =>
        int.TryParse(value, NumberStyles.None, CultureInfo.InvariantCulture, out count);
}
