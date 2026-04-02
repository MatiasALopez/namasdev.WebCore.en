namespace namasdev.WebCore.Models
{
    public class Pagination
    {
        public Pagination(int page, int pageItemCount, int totalItemCount, int totalPageCount)
        {
            Page = page > 0 ? page : 1;
            PageItemCount = pageItemCount;
            TotalItemCount = totalItemCount;
            TotalPageCount = totalPageCount;
        }

        public int Page { get; private set; }
        public int PageItemCount { get; private set; }
        public int TotalItemCount { get; private set; }
        public int TotalPageCount { get; private set; }

        public bool IsSinglePage
        {
            get { return TotalPageCount == 1; }
        }

        public bool PreviousPageAvailable
        {
            get { return Page > 1; }
        }

        public bool NextPageAvailable
        {
            get { return Page < TotalPageCount; }
        }

        public int PreviousPage
        {
            get
            {
                return PreviousPageAvailable
                    ? Page - 1
                    : 1;
            }
        }

        public int NextPage
        {
            get
            {
                return NextPageAvailable
                    ? Page + 1
                    : TotalPageCount;
            }
        }
    }
}
