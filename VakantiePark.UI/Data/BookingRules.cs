using System.Text.RegularExpressions;

namespace VakantiePark.UI.Data;

public static partial class BookingRules
{
    public const int MaxPets = 2;
    public const string ArrivalTime = "15:00";
    public const string DepartureTime = "10:00";

    public static int MaxPersons(AccommodationType type)
    {
        var numbers = Digits().Matches(type.Persons);
        return numbers.Count == 0 ? 0 : int.Parse(numbers[^1].Value);
    }

    [GeneratedRegex(@"\d+")]
    private static partial Regex Digits();
}
