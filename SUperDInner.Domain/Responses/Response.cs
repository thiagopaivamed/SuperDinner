using System.Text.Json.Serialization;

namespace SuperDinner.Domain.Responses
{
    public class Response<TData>
    {
        private readonly int _responseStatusCode;

        [JsonConstructor]
        public Response(int responseStatusCode, TData data) => _responseStatusCode = Configuration.DefaultStatusCode;

        public Response(TData data, int responseStatusCode = Configuration.DefaultStatusCode, string? message = null)
        {
            Data = data;
            _responseStatusCode = responseStatusCode;
            Message = message;
        }

        public TData? Data { get; protected set; }
        public string? Message { get; protected set; }

        [JsonIgnore]
        public bool IsSuccess => _responseStatusCode is >= 200 and <= 299;
    }
}
