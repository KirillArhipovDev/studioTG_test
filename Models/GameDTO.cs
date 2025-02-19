namespace WebApi.Models;

public class GameDTO
{
    public Guid GameId { get; } = Guid.NewGuid();
    public required int Width { get; set; }
    public required int Height { get; set; }
    public required int MinesCount { get; set; }
    public bool Completed { get; set; }
    public required string[][] Field { get; set; }
    public required bool[][] Revealed { get; set; }
    public required bool[][] Mines { get; set; }
}