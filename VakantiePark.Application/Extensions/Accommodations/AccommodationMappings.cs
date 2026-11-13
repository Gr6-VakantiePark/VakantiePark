using VakantiePark.Domain.Entities;
using VakantiePark.Domain.Enums;
using VakantiePark.Shared.DTOs.Accommodations;

namespace VakantiePark.Application.Extensions.Accommodations
{
    public static class AccommodationMappings
    {
        public static AccommodationDto ToDto(this Accommodation a) => new()
        {
            Id = a.Id,
            Code = a.Code,
            Name = a.Name,
            Type = a.AccommodationType.Name,
            TypeSlug = a.AccommodationType.Slug,
            Category = a.AccommodationType.Category.ToString(),
            MaxPersons = a.MaxPersons,
            Zone = a.Zone,
            PricePerNight = a.AccommodationType.PricePerNight,
            MainPhoto = PhotoUrls(a.AccommodationType).FirstOrDefault(),
        };

        public static AccommodationDetailDto ToDetailDto(this Accommodation a) => new()
        {
            Id = a.Id,
            Code = a.Code,
            Name = a.Name,
            Type = a.AccommodationType.Name,
            TypeSlug = a.AccommodationType.Slug,
            Category = a.AccommodationType.Category.ToString(),
            MaxPersons = a.MaxPersons,
            Zone = a.Zone,
            PricePerNight = a.AccommodationType.PricePerNight,
            MainPhoto = PhotoUrls(a.AccommodationType).FirstOrDefault(),

            Description = a.AccommodationType.Description,
            MaxPets = a.MaxPets,
            Bedrooms = a.Bedrooms,
            Bathrooms = a.Bathrooms,
            Beds = a.Beds,
            ElectricityAmps = a.ElectricityAmps,
            MaxVehicleLengthMeters = a.MaxVehicleLengthMeters,
            MaxCaravanLengthMeters = a.MaxCaravanLengthMeters,
            Surface = a.Surface,
            Shade = a.Shade,
            WifiCoverage = a.WifiCoverage,
            Photos = PhotoUrls(a.AccommodationType),
            Amenities = a.Amenities
                .OrderBy(v => v.Name)
                .Select(v => new AmenityDto { Id = v.Id, Code = v.Code, Name = v.Name })
                .ToList(),
        };

        public static List<AccommodationTypeSummaryDto> ToSummaryDtos(this IEnumerable<AccommodationType> types) =>
            types
                .Select(t => t.ToSummaryDto())
                .OrderBy(t => t.MinPersons)
                .ThenByDescending(t => t.UnitCount)
                .ThenBy(t => t.Name)
                .ToList();

        public static List<ZoneDto> ToParkZoneDtos(this IEnumerable<AccommodationType> types) =>
            types
                .GroupBy(t => t.Category)
                .OrderBy(g => g.Key)
                .SelectMany(g => g
                    .SelectMany(t => t.Accommodations)
                    .ToZoneDtos(g.Key.ToString())
                    .OrderBy(z => z.Name))
                .ToList();

        public static SearchOptionsDto ToSearchOptionsDto(
            this List<AccommodationType> types,
            List<Amenity> amenities,
            Dictionary<Guid, List<AccommodationCategory>> categoriesPerAmenity,
            int maxPersons,
            int maxPets,
            ParkSettings? settings) => new()
        {
            Categories = types.Select(t => t.Category).Distinct().Order().Select(c => c.ToString()).ToList(),
            Types = types
                .Select(t => new AccommodationTypeDto { Name = t.Name, Slug = t.Slug, Category = t.Category.ToString() })
                .ToList(),
            Amenities = amenities
                .Where(v => categoriesPerAmenity.ContainsKey(v.Id))
                .Select(v => new AmenityOptionDto
                {
                    Id = v.Id,
                    Code = v.Code,
                    Name = v.Name,
                    Categories = categoriesPerAmenity[v.Id].Select(c => c.ToString()).ToList(),
                })
                .ToList(),
            MaxPersons = maxPersons,
            MaxPets = maxPets,
            TouristTaxMinimumAge = settings?.TouristTaxMinimumAge ?? SearchOptionsDto.DefaultTouristTaxMinimumAge,
        };

        public static AccommodationTypeSummaryDto ToSummaryDto(this AccommodationType type)
        {
            var dto = new AccommodationTypeSummaryDto();
            FillSummary(dto, type);
            return dto;
        }

        public static AccommodationTypeDetailDto ToDetailDto(this AccommodationType type)
        {
            var accommodations = type.Accommodations.ToList();
            var lengths = accommodations
                .Select(a => a.MaxVehicleLengthMeters ?? a.MaxCaravanLengthMeters)
                .Where(length => length != null)
                .ToList();

            var dto = new AccommodationTypeDetailDto
            {
                Description = type.Description,
                MinBedrooms = accommodations.Min(a => a.Bedrooms),
                MaxBedrooms = accommodations.Max(a => a.Bedrooms),
                MinBathrooms = accommodations.Min(a => a.Bathrooms),
                MaxBathrooms = accommodations.Max(a => a.Bathrooms),
                MinBeds = accommodations.Min(a => a.Beds),
                MaxBeds = accommodations.Max(a => a.Beds),
                MaxPets = accommodations.Count == 0 ? 0 : accommodations.Max(a => a.MaxPets),
                MinElectricityAmps = accommodations.Min(a => a.ElectricityAmps),
                MaxElectricityAmps = accommodations.Max(a => a.ElectricityAmps),
                MinLengthMeters = lengths.Min(),
                MaxLengthMeters = lengths.Max(),
                Surfaces = accommodations
                    .Where(a => !string.IsNullOrWhiteSpace(a.Surface))
                    .Select(a => a.Surface!)
                    .Distinct()
                    .Order()
                    .ToList(),
                Amenities = accommodations
                    .SelectMany(a => a.Amenities)
                    .GroupBy(v => v.Id)
                    .Select(g => new TypeAmenityDto
                    {
                        Id = g.Key,
                        Code = g.First().Code,
                        Name = g.First().Name,
                        UnitCount = g.Count(),
                    })
                    .OrderBy(v => v.Name)
                    .ToList(),
                Zones = accommodations.ToZoneDtos(type.Category.ToString()),
            };

            FillSummary(dto, type);
            return dto;
        }

        public static List<ZoneDto> ToZoneDtos(this IEnumerable<Accommodation> accommodations, string category) =>
            accommodations
                .GroupBy(a => a.Zone)
                .Select(g => new ZoneDto { Name = g.Key, Category = category, UnitCount = g.Count() })
                .OrderByDescending(z => z.UnitCount)
                .ThenBy(z => z.Name)
                .ToList();

        private static void FillSummary(AccommodationTypeSummaryDto dto, AccommodationType type)
        {
            dto.Slug = type.Slug;
            dto.Name = type.Name;
            dto.Category = type.Category.ToString();
            dto.Tagline = type.Tagline;
            dto.UnitCount = type.Accommodations.Count;
            dto.MinPersons = type.Accommodations.Count == 0 ? 0 : type.Accommodations.Min(a => a.MaxPersons);
            dto.MaxPersons = type.Accommodations.Count == 0 ? 0 : type.Accommodations.Max(a => a.MaxPersons);
        }

        private static List<string> PhotoUrls(AccommodationType type) =>
            type.Photos
                .OrderBy(f => f.SortOrder)
                .Select(f => $"photos/{f.Id}")
                .ToList();
    }
}
