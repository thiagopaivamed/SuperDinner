using System.Text.Json.Serialization;

namespace SuperDinner.Domain.Responses
{
    public sealed record PagedResponse<TData> : Response<TData>
    {
        [JsonConstructor]
        public PagedResponse(TData data, int totalCount, int currentPage = 1, int pageSize = Configuration.DefaultPageSize) : base(data)
        {
            Data = data;
            TotalCount = totalCount;
            CurrentPage = currentPage;
            PageSize = pageSize;
        }

        public PagedResponse(TData data, int responseStatusCode = 200, List<string?> message = null) : 
            base(data, responseStatusCode, message) { }

        public int TotalCount { get; set; }
        public int CurrentPage { get; set; }
        public int PageSize { get; set; } = Configuration.DefaultPageSize;
        public int TotalPages => (int)Math.Ceiling(TotalCount / (double)PageSize);
    }
}
