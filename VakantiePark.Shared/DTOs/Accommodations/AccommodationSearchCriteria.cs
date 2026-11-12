using VakantiePark.Shared.Common.Pagination;

namespace VakantiePark.Shared.DTOs.Accommodations
{
    public class AccommodationSearchCriteria : Pagination<AccommodationOrderBy>
    {
        public const int DefaultPageSize = 12;
        public const AccommodationOrderBy DefaultOrderBy = AccommodationOrderBy.MaxPersons;
        public const OrderType DefaultOrderType = OrderType.Ascending;

        public AccommodationSearchCriteria()
        {
            PageSize = DefaultPageSize;
            OrderBy = DefaultOrderBy;
            OrderType = DefaultOrderType;
        }

        public DateOnly Arrival { get; set; }
        public DateOnly Departure { get; set; }
        public int Persons { get; set; }

        public int YoungChildren { get; set; }

        public string? Category { get; set; }

        public string? Type { get; set; }

        public int Pets { get; set; }

        public string? Amenities { get; set; }

        public int Nights => Departure.DayNumber - Arrival.DayNumber;

        public List<string> AmenityCodes =>
            (Amenities ?? string.Empty)
                .Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries)
                .ToList();

        public bool HasFilters =>
            !string.IsNullOrWhiteSpace(Category)
            || !string.IsNullOrWhiteSpace(Type)
            || Pets > 0
            || AmenityCodes.Count > 0;

        public AccommodationSearchCriteria WithPage(int pageNumber) => Copy(c => c.PageNumber = pageNumber);

        public AccommodationSearchCriteria WithoutFilters() =>
            Copy(c =>
            {
                c.Category = null;
                c.Type = null;
                c.Pets = 0;
                c.Amenities = null;
                c.PageNumber = 1;
            });

        public AccommodationSearchCriteria WithSorting(AccommodationOrderBy orderBy, OrderType orderType) =>
            Copy(c =>
            {
                c.OrderBy = orderBy;
                c.OrderType = orderType;
                c.PageNumber = 1;
            });

        public string ToQueryString()
        {
            var parts = new List<string>
            {
                $"arrival={Arrival:yyyy-MM-dd}",
                $"departure={Departure:yyyy-MM-dd}",
                $"persons={Persons}",
            };
            if (YoungChildren > 0)
            {
                parts.Add($"youngChildren={YoungChildren}");
            }

            if (!string.IsNullOrWhiteSpace(Category))
            {
                parts.Add($"category={Uri.EscapeDataString(Category)}");
            }

            if (!string.IsNullOrWhiteSpace(Type))
            {
                parts.Add($"type={Uri.EscapeDataString(Type)}");
            }

            if (Pets > 0)
            {
                parts.Add($"pets={Pets}");
            }

            if (!string.IsNullOrWhiteSpace(Amenities))
            {
                parts.Add($"amenities={Uri.EscapeDataString(Amenities)}");
            }

            if (OrderBy != DefaultOrderBy)
            {
                parts.Add($"orderBy={OrderBy}");
            }

            if (OrderType != DefaultOrderType)
            {
                parts.Add($"orderType={OrderType}");
            }

            if (PageNumber > 1)
            {
                parts.Add($"pageNumber={PageNumber}");
            }

            if (PageSize != DefaultPageSize)
            {
                parts.Add($"pageSize={PageSize}");
            }

            return string.Join("&", parts);
        }

        private AccommodationSearchCriteria Copy(Action<AccommodationSearchCriteria> change)
        {
            var copy = (AccommodationSearchCriteria)MemberwiseClone();
            change(copy);
            return copy;
        }
    }
}
