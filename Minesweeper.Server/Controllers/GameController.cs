using Microsoft.AspNetCore.Mvc;
using Minesweeper.Server.Entities;
using Minesweeper.Server.Enums;
using Minesweeper.Server.Requests;
using Minesweeper.Server.Responses;
using Minesweeper.Server.Services;

namespace Minesweeper.Server.Controllers;

[ApiController]
[Route("[controller]")]
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

    /*[HttpGet("field/{id:guid}/{x:int}/{y:int}")]
    public Field GetField(Guid id, int x, int y)
    {
        return _gameService.GetField(id, new Position(x, y));
    }

    [HttpGet("fields/{id:guid}")]
    public List<Field> GetFields(Guid id)
    {
        List<Field> fields = new List<Field>();
        for (var i = 0; i < 9; i++)
        {
            for (var j = 0; j < 9; j++)
            {
                fields.Add(_gameService.GetField(id, new Position(i, j)));
            }
        }
        return fields;
    }*/

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
        return new RevealResponse(fieldClickType, gameState, field.BombCount, clearedFields);
    }
}