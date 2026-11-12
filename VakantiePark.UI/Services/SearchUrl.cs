using VakantiePark.Shared.Common.Pagination;
using VakantiePark.Shared.DTOs.Accommodations;
using VakantiePark.UI.Data;

namespace VakantiePark.UI.Services;

public static class SearchUrl
{
    public const string Page = "zoek-en-boek";

    private static readonly Dictionary<AccommodationCategory, string> CategorySlugs = new()
    {
        [AccommodationCategory.HolidayHome] = "vakantiehuis",
        [AccommodationCategory.CampingPitch] = "kampeerplaats",
    };

    private static readonly Dictionary<AccommodationOrderBy, string> OrderBySlugs = new()
    {
        [AccommodationOrderBy.MaxPersons] = "capaciteit",
        [AccommodationOrderBy.Name] = "naam",
        [AccommodationOrderBy.Type] = "type",
        [AccommodationOrderBy.Zone] = "zone",
    };

    private static readonly Dictionary<OrderType, string> OrderTypeSlugs = new()
    {
        [OrderType.Ascending] = "oplopend",
        [OrderType.Descending] = "aflopend",
    };

    public static string For(AccommodationSearchCriteria criteria)
    {
        var parts = new List<string>
        {
            $"aankomst={criteria.Arrival:yyyy-MM-dd}",
            $"vertrek={criteria.Departure:yyyy-MM-dd}",
            $"personen={criteria.Persons}",
        };

        if (Enum.TryParse<AccommodationCategory>(criteria.Category, ignoreCase: true, out var category))
            parts.Add($"soort={CategorySlugs[category]}");
        if (!string.IsNullOrWhiteSpace(criteria.Type))
            parts.Add($"type={Uri.EscapeDataString(criteria.Type)}");
        if (criteria.Pets > 0)
            parts.Add($"huisdieren={criteria.Pets}");
        if (!string.IsNullOrWhiteSpace(criteria.Amenities))
            parts.Add($"voorzieningen={Uri.EscapeDataString(criteria.Amenities)}");
        if (criteria.OrderBy != AccommodationSearchCriteria.DefaultOrderBy)
            parts.Add($"sorteren={OrderBySlugs[criteria.OrderBy]}");
        if (criteria.OrderType != AccommodationSearchCriteria.DefaultOrderType)
            parts.Add($"volgorde={OrderTypeSlugs[criteria.OrderType]}");
        if (criteria.PageNumber > 1)
            parts.Add($"pagina={criteria.PageNumber}");

        return $"{Page}?{string.Join("&", parts)}";
    }

    public static string? CategoryFromSlug(string? slug) =>
        CategorySlugs.FirstOrDefault(pair => pair.Value.Equals(slug, StringComparison.OrdinalIgnoreCase)) is { Value: not null } match
            ? match.Key.ToString()
            : null;

    public static AccommodationOrderBy? OrderByFromSlug(string? slug) =>
        OrderBySlugs.Where(pair => pair.Value.Equals(slug, StringComparison.OrdinalIgnoreCase))
            .Select(pair => (AccommodationOrderBy?)pair.Key)
            .FirstOrDefault();

    public static OrderType? OrderTypeFromSlug(string? slug) =>
        OrderTypeSlugs.Where(pair => pair.Value.Equals(slug, StringComparison.OrdinalIgnoreCase))
            .Select(pair => (OrderType?)pair.Key)
            .FirstOrDefault();
}
