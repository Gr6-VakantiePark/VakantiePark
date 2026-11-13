using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using VakantiePark.Domain.Interfaces;
using VakantiePark.Domain.Entities;
using VakantiePark.Domain.Enums;
using VakantiePark.Infrastructure.Data;
using VakantiePark.Shared.DTOs.Accommodations;
using VakantiePark.Shared.Common.Pagination;

namespace VakantiePark.Infrastructure.Repositories
{

    public class AccommodationRepository(ApplicationDBContext context) : IAccommodationRepository
    {
        public Task<List<Accommodation>> GetAllAsync() =>
            context.Accommodations
                .AsNoTracking()
                .Include(a => a.AccommodationType).ThenInclude(t => t.Photos)
                .OrderBy(a => a.Code)
                .ToListAsync();



        public Task<Accommodation?> GetByIdAsync(Guid id) =>
            context.Accommodations
                .AsNoTracking()
                .Include(a => a.AccommodationType).ThenInclude(t => t.Photos)
                .Include(a => a.Amenities)
                .FirstOrDefaultAsync(a => a.Id == id);

        public Task<List<AccommodationType>> GetTypesAsync() =>
            context.AccommodationTypes
                .AsNoTracking()
                .OrderBy(t => t.Category)
                .ThenBy(t => t.Name)
                .ToListAsync();

        public Task<List<AccommodationType>> GetTypesWithAccommodationsAsync() =>
            context.AccommodationTypes
                .AsNoTracking()
                .Include(t => t.Accommodations)
                .OrderBy(t => t.Category)
                .ThenBy(t => t.Name)
                .ToListAsync();

        public Task<AccommodationType?> GetTypeBySlugAsync(string slug)
        {
            var normalized = slug.Trim().ToLowerInvariant();
            return context.AccommodationTypes
                .AsNoTracking()
                .Include(t => t.Accommodations).ThenInclude(a => a.Amenities)
                .FirstOrDefaultAsync(t => t.Slug == normalized);
        }

        public Task<int> CountWithAmenityAsync(string amenityCode) =>
            context.Accommodations.CountAsync(a => a.Amenities.Any(v => v.Code == amenityCode));

        public Task<List<Amenity>> GetAmenitiesAsync() =>
            context.Amenities
                .AsNoTracking()
                .OrderBy(v => v.Name)
                .ToListAsync();

        public async Task<Dictionary<Guid, List<AccommodationCategory>>> GetAmenityCategoriesAsync()
        {
            var perAccommodation = await context.Accommodations
                .Select(a => new
                {
                    a.AccommodationType.Category,
                    AmenityIds = a.Amenities.Select(v => v.Id).ToList(),
                })
                .ToListAsync();

            return perAccommodation
                .SelectMany(a => a.AmenityIds.Select(id => (Id: id, a.Category)))
                .GroupBy(p => p.Id)
                .ToDictionary(g => g.Key, g => g.Select(p => p.Category).Distinct().OrderBy(c => c).ToList());
        }



        public async Task<(int MaxPersons, int MaxPets)> GetMaximumsAsync()
        {
            if (!await context.Accommodations.AnyAsync())
                return (0, 0);

            return (await context.Accommodations.MaxAsync(a => a.MaxPersons),
                    await context.Accommodations.MaxAsync(a => a.MaxPets));
        }

        public async Task<(List<Accommodation> Items, int TotalCount)> SearchAsync(AccommodationSearchCriteria pagination)
        {
            ArgumentNullException.ThrowIfNull(pagination);

            var query = context.Accommodations
                .AsNoTracking()
                .Where(a => a.MaxPersons >= pagination.Persons && a.MaxPets >= pagination.Pets);

            if (Enum.TryParse<AccommodationCategory>(pagination.Category, ignoreCase: true, out var category))
                query = query.Where(a => a.AccommodationType.Category == category);

            if (!string.IsNullOrWhiteSpace(pagination.Type))
            {
                string slug = pagination.Type.Trim().ToLowerInvariant();
                query = query.Where(a => a.AccommodationType.Slug == slug);
            }

            foreach (string code in pagination.AmenityCodes)
                query = query.Where(a => a.Amenities.Any(v => v.Code == code));

            int totalCount = await query.CountAsync();

            var sorted = pagination.OrderBy switch
            {
                AccommodationOrderBy.Name => Sort(query, a => a.Name, pagination.OrderType),
                AccommodationOrderBy.Type => Sort(query, a => a.AccommodationType.Name, pagination.OrderType).ThenBy(a => a.Name),
                AccommodationOrderBy.Zone => Sort(query, a => a.Zone, pagination.OrderType).ThenBy(a => a.Name),

                _ => Sort(query, a => a.MaxPersons, pagination.OrderType).ThenBy(a => a.Name),
            };

            var items = await sorted
                .ThenBy(a => a.Id)
                .Skip((pagination.PageNumber - 1) * pagination.PageSize)
                .Take(pagination.PageSize)
                .Include(a => a.AccommodationType).ThenInclude(t => t.Photos)
                .Include(a => a.Amenities)
                .AsSplitQuery()
                .ToListAsync();

            return (items, totalCount);
        }

        private static IOrderedQueryable<T> Sort<T, TKey>(IQueryable<T> query, Expression<Func<T, TKey>> key, OrderType order)
            => order == OrderType.Ascending ? query.OrderBy(key) : query.OrderByDescending(key);
    }
}

