namespace NetCoreEx.Utilities
{
    public abstract class PagedResultBase
    {
        public int CurrentPage { get; set; }

        public int PageCount
        {
            get
            {
                var pageCount = (double)RowCount / PageSize;
                return (int)Math.Ceiling(pageCount);
            }
            set => PageCount = value;
        }

        public int PageSize { get; set; }

        public int RowCount { get; set; }

        public int FirstRowOnPage => Math.Max(CurrentPage - 3, 1);

        public int LastRowOnPage => Math.Min(CurrentPage + 3, PageCount);
    }

    public class PagedResult<T> : PagedResultBase where T : class
    {
        public PagedResult()
        {
            Results = new List<T>();
        }

        public IList<T> Results { get; set; }
    }
}