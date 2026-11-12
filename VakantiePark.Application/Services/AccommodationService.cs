using VakantiePark.Application.Calculators;
using VakantiePark.Application.Extensions;
using VakantiePark.Application.Interfaces;
using VakantiePark.Domain.Entities;
using VakantiePark.Shared.Common;
using VakantiePark.Shared.DTOs.Accommodations;
using VakantiePark.Shared.DTOs.Prices;

namespace VakantiePark.Application.Services
{
    public class AccommodationService(IAccommodationRepository repository, IParkSettingsRepository settingsRepository) : IAccommodationService
    {
        public async Task<List<AccommodationDto>> GetAllAsync()
        {
            var accommodations = await repository.GetAllAsync();
            return accommodations.Select(a => a.ToDto()).ToList();
        }

        public async Task<AccommodationDetailDto> GetByIdAsync(Guid id, DateOnly? arrival = null, DateOnly? departure = null, int? persons = null)
        {
            if (id == Guid.Empty)
                throw new ArgumentException("Het id van de accommodatie ontbreekt.");

            bool hasStay = arrival is not null || departure is not null || persons is not null;
            if (hasStay)
                AccommodationValidation.ValidateStay(arrival, departure, persons);

            var accommodation = await repository.GetByIdAsync(id)
                ?? throw new KeyNotFoundException($"Accommodatie met id {id} is niet gevonden.");

            var dto = accommodation.ToDetailDto();
            if (!hasStay)
                return dto;

            if (persons > accommodation.MaxPersons)
                throw new ArgumentException($"{accommodation.Name} is geschikt voor maximaal {accommodation.MaxPersons} personen.");

            var settings = await settingsRepository.GetAsync();
            dto.Price = PriceFor(accommodation, arrival!.Value, departure!.Value, persons!.Value, settings);
            return dto;
        }

        public async Task<SearchOptionsDto> GetSearchOptionsAsync()
        {
            var types = await repository.GetTypesAsync();
            var amenities = await repository.GetAmenitiesAsync();
            var categoriesPerAmenity = await repository.GetAmenityCategoriesAsync();
            var (maxPersons, maxPets) = await repository.GetMaximumsAsync();

            return new SearchOptionsDto
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
            };
        }

        public async Task<PaginatedResult<AccommodationDetailDto>> SearchAsync(AccommodationSearchCriteria criteria)
        {
            criteria.Validate();

            var (accommodations, totalCount) = await repository.SearchAsync(criteria);
            var settings = await settingsRepository.GetAsync();

            return new PaginatedResult<AccommodationDetailDto>
            {
                Items = accommodations.Select(a =>
                {
                    var dto = a.ToDetailDto();
                    dto.Price = PriceFor(a, criteria.Arrival, criteria.Departure, criteria.Persons, settings);
                    return dto;
                }).ToList(),
                TotalCount = totalCount,
                PageNumber = criteria.PageNumber,
                PageSize = criteria.PageSize,
            };
        }

        private static PriceDto? PriceFor(Accommodation accommodation, DateOnly arrival, DateOnly departure, int persons, ParkSettings? settings)
        {
            if (settings is null || accommodation.AccommodationType.PricePerNight <= 0m)
                return null;

            return PriceCalculator.Calculate(
                accommodation.AccommodationType.PricePerNight,
                departure.DayNumber - arrival.DayNumber,
                persons,
                settings.CleaningFee,
                settings.TouristTaxPerPersonPerNight);
        }
    }
}
