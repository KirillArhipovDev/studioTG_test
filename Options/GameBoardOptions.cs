namespace WebApi.Options;

public class GameBoardOptions
{
    public int MinWidth { get; set; } = 0;
    public int MinHeight { get; set; } = 0;
    public int MaxWidth { get; set; } = 30;
    public int MaxHeight { get; set; } = 30;
    public int MinMinesCount { get; set; } = 1;
}
