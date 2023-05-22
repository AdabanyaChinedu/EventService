namespace EventService.SharedLibrary.Model.ResponseModel
{
    public class PaginatedResult<T> : Result<T>
    {
        public PageParams PageParams { get; set; } = default!;
    }

    public class PageParams
    {
        public PageParams(int pageIndex, int pageSize, int total)
        {
            this.pageIndex = pageIndex;
            this.pageSize = pageSize;
            Total = total;
        }

        public int pageIndex { get; set; }
        public int pageSize { get; set; }
        public int Total { get; set; }
    }
}
