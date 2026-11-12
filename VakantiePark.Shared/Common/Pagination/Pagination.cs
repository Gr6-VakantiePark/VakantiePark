namespace VakantiePark.Shared.Common.Pagination
{
    public abstract class PaginationBase
    {
        public const int MaxPageSize = 50;

        public int PageSize { get; set; } = 10;
        public int PageNumber { get; set; } = 1;
        public OrderType OrderType { get; set; }
    }

    public class Pagination<TOrderBy> : PaginationBase where TOrderBy : Enum
    {
        public TOrderBy OrderBy { get; set; } = default!;
    }

    public enum OrderType
    {
        Descending,
        Ascending
    }
}
