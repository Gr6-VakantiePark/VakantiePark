namespace VakantiePark.Shared.Common
{
    public class PaginatedResult<T>
    {
        public List<T> Items { get; set; } = new();
        public int TotalCount { get; set; }
        public int PageNumber { get; set; }
        public int PageSize { get; set; }

        public int TotalPages => PageSize <= 0 ? 0 : (int)Math.Ceiling(TotalCount / (double)PageSize);
        public int First => Items.Count == 0 ? 0 : (PageNumber - 1) * PageSize + 1;
        public int Last => Items.Count == 0 ? 0 : First + Items.Count - 1;
    }
}
