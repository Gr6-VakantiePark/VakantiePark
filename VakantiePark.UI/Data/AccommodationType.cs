namespace VakantiePark.UI.Data;

public sealed record AccommodationType(
    string Slug,
    string Name,
    AccommodationCategory Category,
    string Tagline,
    string Description,
    string Persons,
    int UnitCount,
    IReadOnlyList<KeyFact> KeyFacts,
    IReadOnlyList<string> Amenities,
    IReadOnlyList<PartialAmenity> PartialAmenities,
    IReadOnlyList<ZoneUnitCount> Zones)
{
    public string Summary =>
        $"{Persons.Replace(" – ", "–")} personen, {UnitCount} {(Category == AccommodationCategory.HolidayHome ? "huizen" : "plaatsen")}";
}
