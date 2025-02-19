using WebApi.Models;

namespace WebApi.Services;

public interface IGameService
{
    GameDTO CreateGame(int width, int height, int minesCount);
    GameDTO? GetGame(Guid gameId);
    bool OpenCell(GameDTO game, int row, int col);
    GameInfoResponse GetGameInfo(GameDTO game);
}
