using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using VakantiePark.Application.Interfaces;
using VakantiePark.Domain.Entities;
using VakantiePark.Domain.Enums;
using VakantiePark.Infrastructure.Data;
using VakantiePark.Shared.Common;
using VakantiePark.Shared.DTOs.Accommodaties;

namespace VakantiePark.Infrastructure.Repositories
{
    public class AccommodatieRepository(ApplicationDBContext context) : IAccommodatieRepository
    {
        public Task<List<Accommodatie>> GetAllAsync() =>
            context.Accommodaties
                .AsNoTracking()
                .Include(a => a.AccommodatieType).ThenInclude(t => t.Afbeeldingen)
                .OrderBy(a => a.Code)
                .ToListAsync();

        public Task<Accommodatie?> GetByIdAsync(int id) =>
            context.Accommodaties
                .AsNoTracking()
                .Include(a => a.AccommodatieType).ThenInclude(t => t.Afbeeldingen)
                .Include(a => a.Voorzieningen)
                .FirstOrDefaultAsync(a => a.Id == id);

        public Task<List<AccommodatieType>> GetTypesAsync() =>
            context.AccommodatieTypes
                .AsNoTracking()
                .OrderBy(t => t.Categorie)
                .ThenBy(t => t.Naam)
                .ToListAsync();

        public Task<List<Voorziening>> GetVoorzieningenAsync() =>
            context.Voorzieningen
                .AsNoTracking()
                .OrderBy(v => v.Naam)
                .ToListAsync();

        public async Task<Dictionary<int, List<AccommodatieCategorie>>> GetVoorzieningCategorieenAsync()
        {
            var perAccommodatie = await context.Accommodaties
                .Select(a => new
                {
                    a.AccommodatieType.Categorie,
                    VoorzieningIds = a.Voorzieningen.Select(v => v.Id).ToList(),
                })
                .ToListAsync();

            return perAccommodatie
                .SelectMany(a => a.VoorzieningIds.Select(id => (Id: id, a.Categorie)))
                .GroupBy(p => p.Id)
                .ToDictionary(g => g.Key, g => g.Select(p => p.Categorie).Distinct().OrderBy(c => c).ToList());
        }

        public async Task<(int MaxPersonen, int MaxHuisdieren)> GetMaximaAsync()
        {
            if (!await context.Accommodaties.AnyAsync())
                return (0, 0);

            return (await context.Accommodaties.MaxAsync(a => a.MaxPersonen),
                    await context.Accommodaties.MaxAsync(a => a.MaxHuisdieren));
        }

        public async Task<(List<Accommodatie> Items, int TotalCount)> ZoekAsync(AccommodatieZoekCriteria pagination)
        {
            var query = context.Accommodaties
                .AsNoTracking()
                .Where(a => a.MaxPersonen >= pagination.Personen && a.MaxHuisdieren >= pagination.Huisdieren);

            if (Enum.TryParse<AccommodatieCategorie>(pagination.Categorie, ignoreCase: true, out var categorie))
                query = query.Where(a => a.AccommodatieType.Categorie == categorie);

            if (!string.IsNullOrWhiteSpace(pagination.Type))
            {
                string slug = pagination.Type.Trim().ToLowerInvariant();
                query = query.Where(a => a.AccommodatieType.Slug == slug);
            }

            foreach (string code in pagination.VoorzieningCodes)
                query = query.Where(a => a.Voorzieningen.Any(v => v.Code == code));

            int totalCount = await query.CountAsync();

            var gesorteerd = pagination.OrderBy switch
            {
                AccommodatieOrderBy.Naam => Sort(query, a => a.Naam, pagination.OrderType),
                AccommodatieOrderBy.Type => Sort(query, a => a.AccommodatieType.Naam, pagination.OrderType).ThenBy(a => a.Naam),
                AccommodatieOrderBy.Zone => Sort(query, a => a.Zone, pagination.OrderType).ThenBy(a => a.Naam),
                _ => Sort(query, a => a.MaxPersonen, pagination.OrderType).ThenBy(a => a.Naam),
            };

            var items = await gesorteerd
                .ThenBy(a => a.Id)
                .Skip((pagination.PageNumber - 1) * pagination.PageSize)
                .Take(pagination.PageSize)
                .Include(a => a.AccommodatieType).ThenInclude(t => t.Afbeeldingen)
                .Include(a => a.Voorzieningen)
                .AsSplitQuery()
                .ToListAsync();

            return (items, totalCount);
        }

        private static IOrderedQueryable<T> Sort<T, TKey>(IQueryable<T> query, Expression<Func<T, TKey>> key, OrderType order)
            => order == OrderType.Ascending ? query.OrderBy(key) : query.OrderByDescending(key);
    }
}
