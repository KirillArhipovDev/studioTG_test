using Microsoft.Extensions.Options;
using WebApi.Factories;
using WebApi.Models;
using WebApi.Options;
using WebApi.Repositorys;

namespace WebApi.Services;

public class GameService(
    IGameFactory gameFactory,
    IGameRepository gameRepository,
    IOptions<CellValueOptions> cellValueOptions
) : IGameService
{
    private readonly IGameFactory _gameFactory = gameFactory;
    private readonly IGameRepository _gameRepository = gameRepository;
    private readonly CellValueOptions _cellValueOptions = cellValueOptions.Value;

    public bool OpenCell(GameDTO game, int row, int col)
    {
        if (row < 0 || row >= game.Height || col < 0 || col >= game.Width || game.Revealed[row][col])
            return false;

        game.Revealed[row][col] = true;

        if (game.Mines[row][col])
        {
            game.Completed = true;
            RevealAllMines(game);
            return true;
        }

        int adjacentMines = CountAdjacentMines(game, row, col);
        game.Field[row][col] = adjacentMines.ToString();

        if (adjacentMines == 0)
            FloodFill(game, row, col);

        if (CheckWinCondition(game))
        {
            game.Completed = true;
            MarkMines(game);
        }

        return true;
    }

    private void RevealAllMines(GameDTO game)
    {
        for (int row = 0; row < game.Height; row++)
        {
            for (int col = 0; col < game.Width; col++)
            {
                if (!game.Mines[row][col])
                {
                    continue;
                }

                game.Field[row][col] = _cellValueOptions.Revealed;
            }
        }

    }

    private void MarkMines(GameDTO game)
    {
        for (int row = 0; row < game.Height; row++)
        {
            for (int col = 0; col < game.Width; col++)
            {
                if (!game.Mines[row][col])
                {
                    continue;
                }

                game.Field[row][col] = _cellValueOptions.Mine;
            }
        }
    }

    private static int CountAdjacentMines(GameDTO game, int row, int col)
    {
        int count = 0;
        for (int dr = -1; dr <= 1; dr++)
        {
            for (int dc = -1; dc <= 1; dc++)
            {
                if (dr != 0 || dc != 0)
                {
                    int newRow = row + dr, newCol = col + dc;
                    if (newRow >= 0 && newRow < game.Height && newCol >= 0 && newCol < game.Width && game.Mines[newRow][newCol])
                    {
                        count++;
                    }
                }
            }
        }

        return count;
    }

    private void FloodFill(GameDTO game, int row, int col)
    {
        for (int dr = -1; dr <= 1; dr++)
        {
            for (int dc = -1; dc <= 1; dc++)
            {
                int newRow = row + dr, newCol = col + dc;
                if (newRow >= 0 && newRow < game.Height && newCol >= 0 && newCol < game.Width && !game.Revealed[newRow][newCol])
                {
                    OpenCell(game, newRow, newCol);
                }
            }
        }
    }

    private static bool CheckWinCondition(GameDTO game) => game.Revealed.Sum(row => row.Count(cell => cell)) + game.MinesCount == game.Width * game.Height;

    public GameInfoResponse GetGameInfo(GameDTO game)
    {
        return new()
        {
            GameId = game.GameId,
            Width = game.Width,
            Height = game.Height,
            MinesCount = game.MinesCount,
            Completed = game.Completed,
            Field = game.Field
        };
    }

    public GameDTO CreateGame(int width, int height, int minesCount)
    {
        var  game = _gameFactory.CreateGame(width, height, minesCount);
        _gameRepository.AddGame(game);
        return game;
    }

    public GameDTO? GetGame(Guid gameId)
    {
        return _gameRepository.GetGame(gameId);
    }
}