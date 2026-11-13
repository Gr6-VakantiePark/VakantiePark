using VakantiePark.Shared.DTOs.Accommodations;

namespace VakantiePark.UI.Data;

public static class AccommodationTypeMappings
{
    public static AccommodationType ToAccommodationType(this AccommodationTypeSummaryDto dto) =>
        new(
            Slug: dto.Slug,
            Name: dto.Name,
            Category: ParseCategory(dto.Category),
            Tagline: dto.Tagline ?? string.Empty,
            Description: string.Empty,
            Persons: Range(dto.MinPersons, dto.MaxPersons),
            UnitCount: dto.UnitCount,
            KeyFacts: [],
            Amenities: [],
            PartialAmenities: [],
            Zones: []);

    public static AccommodationType ToAccommodationType(this AccommodationTypeDetailDto dto)
    {
        var category = ParseCategory(dto.Category);

        return new(
            Slug: dto.Slug,
            Name: dto.Name,
            Category: category,
            Tagline: dto.Tagline ?? string.Empty,
            Description: dto.Description ?? string.Empty,
            Persons: Range(dto.MinPersons, dto.MaxPersons),
            UnitCount: dto.UnitCount,
            KeyFacts: KeyFacts(dto, category),
            Amenities: dto.Amenities
                .Where(amenity => amenity.UnitCount >= dto.UnitCount)
                .Select(amenity => amenity.Name)
                .ToList(),
            PartialAmenities: dto.Amenities
                .Where(amenity => amenity.UnitCount < dto.UnitCount)
                .Select(amenity => new PartialAmenity(amenity.Name, amenity.UnitCount))
                .ToList(),
            Zones: dto.Zones
                .Select(zone => new ZoneUnitCount(zone.Name, zone.UnitCount))
                .ToList());
    }

    public static AccommodationCategory ParseCategory(string category)
    {
        if (Enum.TryParse<AccommodationCategory>(category, true, out var parsed))
        {
            return parsed;
        }

        return AccommodationCategory.HolidayHome;
    }

    private static List<KeyFact> KeyFacts(AccommodationTypeDetailDto dto, AccommodationCategory category)
    {
        var facts = new List<KeyFact>
        {
            new("Personen", Range(dto.MinPersons, dto.MaxPersons)),
        };

        if (category == AccommodationCategory.HolidayHome)
        {
            if (dto.MinBedrooms != null && dto.MaxBedrooms != null)
            {
                facts.Add(new("Slaapkamers", Range(dto.MinBedrooms.Value, dto.MaxBedrooms.Value)));
            }

            if (dto.MinBathrooms != null && dto.MaxBathrooms != null)
            {
                facts.Add(new("Badkamers", Range(dto.MinBathrooms.Value, dto.MaxBathrooms.Value)));
            }

            if (dto.MinBeds != null && dto.MaxBeds != null)
            {
                facts.Add(new("Bedden", Range(dto.MinBeds.Value, dto.MaxBeds.Value)));
            }
        }
        else
        {
            if (dto.Surfaces.Count > 0)
            {
                facts.Add(new("Ondergrond", string.Join(", ", dto.Surfaces)));
            }

            if (dto.MinElectricityAmps != null && dto.MaxElectricityAmps != null)
            {
                facts.Add(new("Stroom", Range(dto.MinElectricityAmps.Value, dto.MaxElectricityAmps.Value, " A")));
            }
            else
            {
                facts.Add(new("Stroom", "Geen"));
            }

            if (dto.MinLengthMeters != null && dto.MaxLengthMeters != null)
            {
                facts.Add(new("Max. lengte", Range(dto.MinLengthMeters.Value, dto.MaxLengthMeters.Value, " m")));
            }
        }

        facts.Add(new("Huisdieren", dto.MaxPets > 0 ? $"max. {dto.MaxPets}" : "Niet toegestaan"));
        return facts;
    }

    private static string Range(int min, int max, string unit = "")
    {
        if (min == max)
        {
            return $"{min.ToString(Formatting.Dutch)}{unit}";
        }

        return $"{min.ToString(Formatting.Dutch)}{unit} – {max.ToString(Formatting.Dutch)}{unit}";
    }

    private static string Range(decimal min, decimal max, string unit)
    {
        if (min == max)
        {
            return $"{min.ToString("0.#", Formatting.Dutch)}{unit}";
        }

        return $"{min.ToString("0.#", Formatting.Dutch)}{unit} – {max.ToString("0.#", Formatting.Dutch)}{unit}";
    }
}
