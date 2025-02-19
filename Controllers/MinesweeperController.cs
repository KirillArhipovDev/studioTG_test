using Microsoft.AspNetCore.Mvc;
using WebApi.Models;
using WebApi.Services;

namespace WebApi.Controllers;

[ApiController]
[Route("api")]
public class MinesweeperController(IGameService gameService) : ControllerBase
{
    private readonly IGameService _gameService = gameService;

    [HttpPost("new")]
    public ActionResult<GameInfoResponse> NewGame([FromBody] NewGameRequest request)
    {
        if (request.Width <= 0 || request.Height <= 0 || request.Width > 30 || request.Height > 30)
        {
            return BadRequest(new ErrorResponse("Некорректные размеры поля."));
        }

        if (request.MinesCount < 1 || request.MinesCount >= request.Width * request.Height)
        {
            return BadRequest(new ErrorResponse("Некорректное количество мин."));
        }

        var game = _gameService.CreateGame(request.Width, request.Height, request.MinesCount);
        return Ok(_gameService.GetGameInfo(game));
    }

    [HttpPost("turn")]
    public ActionResult<GameInfoResponse> MakeTurn([FromBody] GameTurnRequest request)
    {
        var game = _gameService.GetGame(request.GameId);
        if (game is null)
        {
            return BadRequest(new ErrorResponse("Игра не найдена."));
        }

        if (game.Completed)
        {
            return BadRequest(new ErrorResponse("Игра уже завершена."));
        }

        if (!_gameService.OpenCell(game, request.Row, request.Col))
        {
            return BadRequest(new ErrorResponse("Некорректный ход."));
        }

        return Ok(_gameService.GetGameInfo(game));
    }
}
