using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using WebApi.Models;
using WebApi.Options;
using WebApi.Services;

namespace WebApi.Controllers;

[ApiController]
[Route("api")]
public class MinesweeperController(
    IGameService gameService,
    IOptions<GameBoardOptions> gameBoardOptions
) : ControllerBase
{
    private readonly IGameService _gameService = gameService;
    private readonly GameBoardOptions _gameBoardOptions = gameBoardOptions.Value;

    [HttpPost("new")]
    public ActionResult<GameInfoResponse> NewGame([FromBody] NewGameRequest request)
    {
        if (ValidateGameBoardSize(request))
        {
            return BadRequest(new ErrorResponse("Некорректные размеры поля."));
        }

        if (ValidateMinesCount(request))
        {
            return BadRequest(new ErrorResponse("Некорректное количество мин."));
        }

        var game = _gameService.CreateGame(request.Width, request.Height, request.MinesCount);
        return Ok(_gameService.GetGameInfo(game));
    }

    private bool ValidateGameBoardSize(NewGameRequest request)
    {
        return request.Width <= _gameBoardOptions.MinWidth ||
            request.Height <= _gameBoardOptions.MinHeight ||
            request.Width > _gameBoardOptions.MaxWidth ||
            request.Height > _gameBoardOptions.MaxHeight;
    }

    private bool ValidateMinesCount(NewGameRequest request)
    {
        return request.MinesCount < _gameBoardOptions.MinMinesCount || request.MinesCount >= request.Width * request.Height;
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
