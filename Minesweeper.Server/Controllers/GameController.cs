using Microsoft.AspNetCore.Mvc;
using Minesweeper.Server.Enums;
using Minesweeper.Server.Requests;
using Minesweeper.Server.Responses;
using Minesweeper.Server.Services;

namespace Minesweeper.Server.Controllers;

[ApiController]
[Route("game")]
public class GameController
{
    private readonly IGameService _gameService;
    private readonly ILogger<GameController> _logger;

    public GameController(ILogger<GameController> logger, IGameService gameService)
    {
        _logger = logger;
        _gameService = gameService;
    }

    [HttpGet("start/{fieldType}")]
    public Guid StartGame(PlayFieldSize fieldType)
    {
        return _gameService.StartGame(fieldType);
    }

    [HttpPost("revealfield")]
    public RevealResponse RevealField(RevealRequest request)
    {
        return _gameService.RevealField(request);
    }

    [HttpGet("revealBombs/{id:guid}")]
    public RevealBombsResponse RevealBombs(Guid id)
    {
        var field = _gameService.GetPlayField(id);
        return new RevealBombsResponse(field.RevealBombs());
    }

}