using Newtonsoft.Json;

namespace WebApi.DTO.Response;

public class ErrorResponse // Move to common
{
    [JsonProperty("message")]
    public string Message { get; set; }

    [JsonProperty("data", NullValueHandling = NullValueHandling.Ignore)]
    public object Data { get; set; }
}