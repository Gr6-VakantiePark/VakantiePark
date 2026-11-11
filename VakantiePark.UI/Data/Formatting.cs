using System.Globalization;

namespace VakantiePark.UI.Data;

public static class Formatting
{
    private static readonly CultureInfo Dutch = new("nl-NL");

    public static string Euro(decimal amount) => amount.ToString("C", Dutch);

    public static string Date(DateOnly date) => date.ToString("dddd d MMMM yyyy", Dutch);
}
