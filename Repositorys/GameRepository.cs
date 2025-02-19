using System.Collections.Concurrent;
using WebApi.Models;

namespace WebApi.Repositorys;

public class GameRepository : IGameRepository
{
    private static readonly ConcurrentDictionary<Guid, GameDTO> _games = new();
    
    public void AddGame(GameDTO game)
    {
        _games[game.GameId] = game;
    }

    public GameDTO? GetGame(Guid gameId)
    {
        _games.TryGetValue(gameId, out var game);
        return game;
    }
}
