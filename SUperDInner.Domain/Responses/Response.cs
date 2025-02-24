using System.Text.Json.Serialization;

namespace SuperDinner.Domain.Responses
{
    public record Response<TData>
    {
        public int ResponseStatusCode { get; init; }
        public TData? Data { get; init; }
        public List<string> Messages { get; init; }

        [JsonIgnore]
        public bool IsSuccess => ResponseStatusCode is >= 200 and <= 299;

        [JsonConstructor]
        public Response(int responseStatusCode, TData? data, List<string>? messages = null)
        {
            ResponseStatusCode = responseStatusCode;
            Data = data;
            Messages = messages ?? new List<string>();
        }

        public Response(TData data, int responseStatusCode = Configuration.DefaultStatusCode, List<string>? messages = null)
            : this(responseStatusCode, data, messages) { }
    }
}
