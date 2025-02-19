using System.Text.Json.Serialization;

namespace WebApi.Models;
public class GameInfoResponse
{
    [JsonPropertyName("game_id")]
    public Guid GameId { get; set; }

    [JsonPropertyName("width")]
    public int Width { get; set; }

    [JsonPropertyName("height")]
    public int Height { get; set; }

    [JsonPropertyName("mines_count")]
    public int MinesCount { get; set; }

    [JsonPropertyName("completed")]
    public bool Completed { get; set; }

    [JsonPropertyName("field")]
    public required string[][] Field { get; set; }
}