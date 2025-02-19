using System.Text.Json.Serialization;

namespace WebApi.Models;

public class ErrorResponse(string message)
{
    [JsonPropertyName("error")]
    public string Error { get; set; } = message;
}
