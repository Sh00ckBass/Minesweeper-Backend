using Minesweeper.Server.Entities;
using Minesweeper.Server.Enums;
using Minesweeper.Server.repository;
using Minesweeper.Server.Requests;
using Minesweeper.Server.Responses;

namespace Minesweeper.Server.Services;

public class GameService : IGameService
{
    private readonly IPlayFieldRepository _playFieldRepository;
    private readonly ILogger<GameService> _logger;

    public GameService(IPlayFieldRepository playFieldRepository, ILogger<GameService> logger)
    {
        _playFieldRepository = playFieldRepository;
        _logger = logger;
    }

    public Guid StartGame(PlayFieldSize fieldType)
    {
        return _playFieldRepository.SetupField(fieldType);
    }

    public Field GetField(Guid id, Position position)
    {
        return _playFieldRepository.GetField(id, position);
    }

    public PlayField GetPlayField(Guid id)
    {
        return _playFieldRepository.GetPlayField(id);
    }
    
    public List<ClearedField>? OnReveal(Guid id, Field field)
    {
        return _playFieldRepository.OnClick(id, field);
    }

    public RevealResponse RevealField(RevealRequest request)
    {
        var field = GetField(request.PlayFieldId, request.Position);
        RevealResult fieldClickType;
        GameState gameState;

        if (field.Bomb)
        {
            field.Visible = true;
            fieldClickType = RevealResult.Bomb;
            gameState = GameState.GameOver;
            return new RevealResponse(fieldClickType, gameState, field.Position);
        }

        var clearedFields = OnReveal(request.PlayFieldId, field);
        fieldClickType = RevealResult.Cleared;
        gameState = GameState.Continue;

        if (GetPlayField(request.PlayFieldId).ClearedCompleteField())
        {
            gameState = GameState.Win;
            return new RevealResponse(fieldClickType, gameState, field.BombCount, clearedFields, field.Position);
        }
        
        return new RevealResponse(fieldClickType, gameState, field.BombCount, clearedFields, field.Position);
    }
    
}