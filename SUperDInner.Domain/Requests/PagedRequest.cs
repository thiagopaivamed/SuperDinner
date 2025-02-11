namespace SuperDinner.Domain.Requests
{
    public abstract record PagedRequest : Request
    {
        public int PageNumber { get; set; } = Configuration.DefaultPageNumber;
        public int PageSize { get; set; } = Configuration.DefaultPageSize;
    }
}
