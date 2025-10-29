using Newtonsoft.Json;

namespace Assecor.Backend.Api.Responses
{
    [JsonObject(ItemNullValueHandling = NullValueHandling.Ignore)]
    public class RestServerErrorResponse()
    {
        public string Title { get; set; } = string.Empty;
        public object? Details { get; set; }
        public int? StatusCode { get; set; }
        public string? TraceId { get; set; }
    }
}
