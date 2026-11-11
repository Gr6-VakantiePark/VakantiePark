using VakantiePark.Domain.Entities;
using VakantiePark.Shared.DTOs.Accommodations;

namespace VakantiePark.Application.Extensions
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

        private static List<string> PhotoUrls(AccommodationType type) =>
            type.Photos
                .OrderBy(f => f.SortOrder)
                .Select(f => $"photos/{f.Id}")
                .ToList();
    }
}
