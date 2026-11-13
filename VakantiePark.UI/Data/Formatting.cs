using System.Globalization;

namespace VakantiePark.UI.Data;

public static class Formatting
{
    public static readonly CultureInfo Dutch = CultureInfo.GetCultureInfo("nl-NL");

    public static string Euro(decimal amount) => amount.ToString("C", Dutch);

    public static string Date(DateOnly date) => date.ToString("dddd d MMMM yyyy", Dutch);

    public static string Iso(DateOnly date) => date.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture);
}
