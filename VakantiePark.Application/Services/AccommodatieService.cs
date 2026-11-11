using VakantiePark.Application.Extensions;
using VakantiePark.Application.Interfaces;
using VakantiePark.Domain.Enums;
using VakantiePark.Shared.Common;
using VakantiePark.Shared.DTOs.Accommodaties;

namespace VakantiePark.Application.Services
{
    public class AccommodatieService(IAccommodatieRepository repository) : IAccommodatieService
    {
        public async Task<List<AccommodatieDto>> GetAllAsync()
        {
            var accommodaties = await repository.GetAllAsync();
            return accommodaties.Select(a => a.ToDto()).ToList();
        }

        public async Task<AccommodatieDetailDto> GetByIdAsync(int id)
        {
            var accommodatie = await repository.GetByIdAsync(id)
                ?? throw new KeyNotFoundException($"Accommodatie met id {id} is niet gevonden.");

            return accommodatie.ToDetailDto();
        }

        public async Task<ZoekOptiesDto> GetZoekOptiesAsync()
        {
            var types = await repository.GetTypesAsync();
            var voorzieningen = await repository.GetVoorzieningenAsync();
            var categorieenPerVoorziening = await repository.GetVoorzieningCategorieenAsync();
            var (maxPersonen, maxHuisdieren) = await repository.GetMaximaAsync();

            return new ZoekOptiesDto
            {
                Categorieen = types.Select(t => t.Categorie).Distinct().Order().Select(c => c.ToString()).ToList(),
                Types = types
                    .Select(t => new AccommodatieTypeDto { Naam = t.Naam, Slug = t.Slug, Categorie = t.Categorie.ToString() })
                    .ToList(),
                Voorzieningen = voorzieningen
                    .Where(v => categorieenPerVoorziening.ContainsKey(v.Id))
                    .Select(v => new VoorzieningOptieDto
                    {
                        Id = v.Id,
                        Code = v.Code,
                        Naam = v.Naam,
                        Categorieen = categorieenPerVoorziening[v.Id].Select(c => c.ToString()).ToList(),
                    })
                    .ToList(),
                MaxPersonen = maxPersonen,
                MaxHuisdieren = maxHuisdieren,
            };
        }

        public async Task<PaginatedResult<AccommodatieDetailDto>> ZoekAsync(AccommodatieZoekCriteria criteria)
        {
            Valideer(criteria);

            var (accommodaties, totalCount) = await repository.ZoekAsync(criteria);

            return new PaginatedResult<AccommodatieDetailDto>
            {
                Items = accommodaties.Select(a => a.ToDetailDto()).ToList(),
                TotalCount = totalCount,
                PageNumber = criteria.PageNumber,
                PageSize = criteria.PageSize,
            };
        }

        private static void Valideer(AccommodatieZoekCriteria c)
        {
            if (c.Aankomst == default || c.Vertrek == default)
                throw new ArgumentException("Vul een aankomst- en vertrekdatum in.");

            if (c.Aankomst < DateOnly.FromDateTime(DateTime.Today))
                throw new ArgumentException("De aankomstdatum mag niet in het verleden liggen.");

            if (c.Vertrek <= c.Aankomst)
                throw new ArgumentException("De vertrekdatum moet na de aankomstdatum liggen.");

            if (c.Personen < 1)
                throw new ArgumentException("Het aantal personen moet minimaal 1 zijn.");

            if (c.Huisdieren < 0)
                throw new ArgumentException("Het aantal huisdieren kan niet negatief zijn.");

            if (c.PageNumber < 1)
                throw new ArgumentException("Het paginanummer moet minimaal 1 zijn.");

            if (c.PageSize < 1 || c.PageSize > PaginationBase.MaxPageSize)
                throw new ArgumentException($"De paginagrootte moet tussen 1 en {PaginationBase.MaxPageSize} liggen.");

            if (!Enum.IsDefined(c.OrderBy))
                throw new ArgumentException($"Op '{c.OrderBy}' kan niet gesorteerd worden.");

            if (!Enum.IsDefined(c.OrderType))
                throw new ArgumentException($"Onbekende sorteerrichting '{c.OrderType}'.");

            if (!string.IsNullOrWhiteSpace(c.Categorie)
                && (!Enum.TryParse<AccommodatieCategorie>(c.Categorie, ignoreCase: true, out var categorie) || !Enum.IsDefined(categorie)))
                throw new ArgumentException($"Onbekende categorie '{c.Categorie}'. Kies Huisje of Campingplaats.");
        }
    }
}
