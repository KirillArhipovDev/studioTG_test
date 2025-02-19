using WebApi.Models;

namespace WebApi.Factories;

public interface IGameFactory
{
    GameDTO CreateGame(int width, int height, int minesCount);
}
