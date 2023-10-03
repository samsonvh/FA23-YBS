namespace YBS.Data.Requests
{
    public abstract class BaseSearchRequest
    {
        public int PageSize { get; set; } = 20;
        public int PageIndex { get; set; } = 1;
        public string SortField { get; set; }
        public bool isSortDesc { get; set; }
    }
}