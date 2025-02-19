using Microsoft.Extensions.Options;
using WebApi.Models;
using WebApi.Options;

namespace WebApi.Factories;

public class GameFactory(IOptions<CellValueOptions> cellValueOptions) : IGameFactory
{
    private readonly CellValueOptions _cellValueOptions = cellValueOptions.Value;

    public GameDTO CreateGame(int width, int height, int minesCount)
    {
        var game = new GameDTO()
        {
            Width = width,
            Height = height,
            MinesCount = minesCount,
            Field = new string[height][],
            Revealed = new bool[height][],
            Mines = new bool[height][]
        };

        for (int row = 0; row < height; row++)
        {
            game.Field[row] = new string[width];
            game.Revealed[row] = new bool[width];
            game.Mines[row] = new bool[width];
            Array.Fill(game.Field[row], _cellValueOptions.Empty);
        }

        PlaceMines(game);

        return game;
    }

    private static void PlaceMines(GameDTO game)
    {
        var rand = new Random();
        int placedMines = 0;

        while (placedMines < game.MinesCount)
        {
            int row = rand.Next(game.Height);
            int col = rand.Next(game.Width);

            if (!game.Mines[row][col])
            {
                game.Mines[row][col] = true;
                placedMines++;
            }
        }
    }
}
