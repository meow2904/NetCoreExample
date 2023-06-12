namespace NetCoreEx.WebApp.Infrastructure.CoreHrm
{
    public class PaginationSet<T> where T : class
    {
        public int Count => Items?.Count() ?? 0;
        public int Page { set; get; }
        public int TotalPages { set; get; }
        public int TotalCount { set; get; }
        public IEnumerable<T> Items { set; get; }
    }

    public class PaginationObject<T> where T : class
    {
        public int Page { set; get; }
        public int PageSize { set; get; }
        public int TotalPages { set; get; }
        public int TotalRow { set; get; }
        public int MaxPage { set; get; }
        public T ObjectFilter { set; get; }
    }

    public class Pagination
    {
        public int Page { set; get; }
        public int PageSize { set; get; }
        public int TotalPages { set; get; }
        public int TotalRow { set; get; }
        public int MaxPage { set; get; }
    }
}
