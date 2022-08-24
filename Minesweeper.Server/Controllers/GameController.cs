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
    private readonly ILogger<WeatherForecastController> _logger;

    public GameController(ILogger<WeatherForecastController> logger, IGameService gameService)
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
        var field = _gameService.GetField(request.PlayFieldId, request.Position);
        RevealResult fieldClickType;
        GameState gameState;

        if (field.Bomb)
        {
            field.Visible = true;
            fieldClickType = RevealResult.Bomb;
            gameState = GameState.GameOver;
            return new RevealResponse(fieldClickType, gameState);
        }

        var clearedFields = _gameService.OnReveal(request.PlayFieldId, field);
        fieldClickType = RevealResult.Cleared;
        gameState = GameState.Continue;

        if (_gameService.GetPlayField(request.PlayFieldId).ClearedCompleteField())
        {
            gameState = GameState.Win;
            return new RevealResponse(fieldClickType, gameState, field.BombCount, clearedFields);
        }
        
        return new RevealResponse(fieldClickType, gameState, field.BombCount, clearedFields);
    }

    [HttpGet("revealBombs/{id:guid}")]
    public RevealBombsResponse RevealBombs(Guid id)
    {
        var field = _gameService.GetPlayField(id);
        return new RevealBombsResponse(field.RevealBombs());
    }

}