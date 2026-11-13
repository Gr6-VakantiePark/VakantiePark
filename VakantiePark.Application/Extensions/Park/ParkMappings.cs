using System.Globalization;
using VakantiePark.Domain.Entities;
using VakantiePark.Domain.Enums;
using VakantiePark.Shared.DTOs.Park;

namespace VakantiePark.Application.Extensions.Park
{
    public static class ParkMappings
    {
        public static ParkInfoDto ToInfoDto(
            this ParkSettings settings,
            List<OpeningHour> openingHours,
            List<PoolBasin> poolBasins,
            int accommodationsNearPool) => new()
        {
            ArrivalTime = settings.ArrivalTime.ToString("HH:mm", CultureInfo.InvariantCulture),
            DepartureTime = settings.DepartureTime.ToString("HH:mm", CultureInfo.InvariantCulture),
            Street = settings.Street,
            PostalCode = settings.PostalCode,
            City = settings.City,
            PhoneNumber = settings.PhoneNumber,
            Email = settings.Email,
            AccommodationsNearPool = accommodationsNearPool,
            ReceptionHours = openingHours.ToDtos(Facility.Reception),
            PoolHours = openingHours.ToDtos(Facility.Pool),
            PoolBasins = poolBasins.Select(p => p.ToDto()).ToList(),
        };

        public static OpeningHourDto ToDto(this OpeningHour openingHour) => new()
        {
            Label = openingHour.Label,
            Value = openingHour.Value,
            ShowOnHome = openingHour.ShowOnHome,
        };

        public static PoolBasinDto ToDto(this PoolBasin poolBasin) => new()
        {
            Name = poolBasin.Name,
            Description = poolBasin.Description,
            Size = poolBasin.Size,
        };

        private static List<OpeningHourDto> ToDtos(this IEnumerable<OpeningHour> openingHours, Facility facility) =>
            openingHours
                .Where(o => o.Facility == facility)
                .OrderBy(o => o.SortOrder)
                .Select(o => o.ToDto())
                .ToList();
    }
}
