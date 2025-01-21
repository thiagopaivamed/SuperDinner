using System.Text.Json.Serialization;

namespace SuperDinner.Domain.Responses
{
    public class Response<TData>
    {
        private readonly int _responseStatusCode;

        [JsonConstructor]
        public Response(int responseStatusCode, TData data) => _responseStatusCode = Configuration.DefaultStatusCode;

        public Response(TData data, int responseStatusCode = Configuration.DefaultStatusCode, List<string> messages = null)
        {
            Data = data;
            _responseStatusCode = responseStatusCode;
            Messages = messages;
        }

        public TData? Data { get; protected set; }
        public List<string> Messages { get; protected set; }

        [JsonIgnore]
        public bool IsSuccess => _responseStatusCode is >= 200 and <= 299;
    }
}
