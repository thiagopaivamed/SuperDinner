using System.Text.Json.Serialization;

namespace SuperDinner.Domain.Responses
{
    public class Response<TData>
    {
        [JsonIgnore]
        public readonly int ResponseStatusCode;

        public TData? Data { get; protected set; }
        public List<string> Messages { get; protected set; }

        [JsonIgnore]
        public bool IsSuccess => ResponseStatusCode is >= 200 and <= 299;

        [JsonConstructor]
        public Response(int statusCode, TData data) => ResponseStatusCode = Configuration.DefaultStatusCode;

        public Response(TData data, int responseStatusCode = Configuration.DefaultStatusCode, List<string> messages = null)
        {
            Data = data;
            ResponseStatusCode = responseStatusCode;
            Messages = messages;
        }
    }
}
