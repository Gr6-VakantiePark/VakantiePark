using VakantiePark.Application.Extensions;
using VakantiePark.Application.Interfaces;
using VakantiePark.Domain.Enums;
using VakantiePark.Shared.Common;
using VakantiePark.Shared.DTOs.Accommodations;

namespace VakantiePark.Application.Services
{
    public class AccommodationService(IAccommodationRepository repository) : IAccommodationService
    {
        public async Task<List<AccommodationDto>> GetAllAsync()
        {
            var accommodations = await repository.GetAllAsync();
            return accommodations.Select(a => a.ToDto()).ToList();
        }

        public async Task<AccommodationDetailDto> GetByIdAsync(int id)
        {
            var accommodation = await repository.GetByIdAsync(id)
                ?? throw new KeyNotFoundException($"Accommodatie met id {id} is niet gevonden.");

            return accommodation.ToDetailDto();
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
            Validate(criteria);

            var (accommodations, totalCount) = await repository.SearchAsync(criteria);

            return new PaginatedResult<AccommodationDetailDto>
            {
                Items = accommodations.Select(a => a.ToDetailDto()).ToList(),
                TotalCount = totalCount,
                PageNumber = criteria.PageNumber,
                PageSize = criteria.PageSize,
            };
        }

        private static void Validate(AccommodationSearchCriteria c)
        {
            if (c.Arrival == default || c.Departure == default)
                throw new ArgumentException("Vul een aankomst- en vertrekdatum in.");

            if (c.Arrival < DateOnly.FromDateTime(DateTime.Today))
                throw new ArgumentException("De aankomstdatum mag niet in het verleden liggen.");

            if (c.Departure <= c.Arrival)
                throw new ArgumentException("De vertrekdatum moet na de aankomstdatum liggen.");

            if (c.Persons < 1)
                throw new ArgumentException("Het aantal personen moet minimaal 1 zijn.");

            if (c.Pets < 0)
                throw new ArgumentException("Het aantal huisdieren kan niet negatief zijn.");

            if (c.PageNumber < 1)
                throw new ArgumentException("Het paginanummer moet minimaal 1 zijn.");

            if (c.PageSize < 1 || c.PageSize > PaginationBase.MaxPageSize)
                throw new ArgumentException($"De paginagrootte moet tussen 1 en {PaginationBase.MaxPageSize} liggen.");

            if (!Enum.IsDefined(c.OrderBy))
                throw new ArgumentException($"Op '{c.OrderBy}' kan niet gesorteerd worden.");

            if (!Enum.IsDefined(c.OrderType))
                throw new ArgumentException($"Onbekende sorteerrichting '{c.OrderType}'.");

            if (!string.IsNullOrWhiteSpace(c.Category)
                && (!Enum.TryParse<AccommodationCategory>(c.Category, ignoreCase: true, out var category) || !Enum.IsDefined(category)))
                throw new ArgumentException($"Onbekende soort verblijf '{c.Category}'.");
        }
    }
}
