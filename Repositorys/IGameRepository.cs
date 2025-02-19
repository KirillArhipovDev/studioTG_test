using WebApi.Models;

namespace WebApi.Repositorys;

public interface IGameRepository
{
    void AddGame(GameDTO game);
    GameDTO? GetGame(Guid gameId);
}
