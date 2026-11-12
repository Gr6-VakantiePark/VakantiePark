namespace VakantiePark.UI.Data;

public static class AccommodationCategoryLabels
{
    public static string Label(AccommodationCategory category) =>
        category == AccommodationCategory.HolidayHome ? "Vakantiehuis" : "Kampeerplaats";

    public static string Label(string? category) =>
        Enum.TryParse<AccommodationCategory>(category, ignoreCase: true, out var parsed) ? Label(parsed) : category ?? string.Empty;
}
