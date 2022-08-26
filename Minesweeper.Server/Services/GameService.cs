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

    public Field GetField(PlayField playField, Position position)
    {
        return _playFieldRepository.GetField(playField, position);
    }

    public PlayField GetPlayFieldMongo(Guid id)
    {
        return _playFieldRepository.GetPlayFieldMongo(id);
    }

    public List<ClearedField> OnReveal(PlayField playField, Field field)
    {
        return _playFieldRepository.OnClick(playField, field);
    }

    public RevealBombsResponse RevealBombs(PlayField playField)
    {
        List<Position> bombs = playField.RevealBombs();
        _playFieldRepository.UpdatePlayField(playField);
        return new RevealBombsResponse(bombs);
    }

    public RevealResponse RevealField(RevealRequest request)
    {
        PlayField playField = GetPlayFieldMongo(request.PlayFieldId);
        var field = GetField(playField, request.Position);
        RevealResult fieldClickType;
        GameState gameState;

        if (field.Bomb)
        {
            field.Visible = true;
            fieldClickType = RevealResult.Bomb;
            gameState = GameState.GameOver;
            
            _playFieldRepository.UpdatePlayField(playField);

            return new RevealResponse(fieldClickType, gameState, field.Position);
        }

        var clearedFields = OnReveal(playField, field);
        fieldClickType = RevealResult.Cleared;
        gameState = GameState.Continue;
        

        if (playField.ClearedCompleteField)
        {
            gameState = GameState.Win;

            _playFieldRepository.UpdatePlayField(playField);

            return new RevealResponse(fieldClickType, gameState, field.BombCount, clearedFields, field.Position);
        }

        _playFieldRepository.UpdatePlayField(playField);

        return new RevealResponse(fieldClickType, gameState, field.BombCount, clearedFields, field.Position);
    }

    public GetSavedPlayFieldResponse GetSavedPlayField(Guid playFieldId)
    {
        PlayField field = GetPlayFieldMongo(playFieldId);
        PlayFieldSize fieldSize = PlayFieldSize.Small;
        switch (field.Size)
        {
            case 16:
            {
                fieldSize = PlayFieldSize.Medium;
                break;
            }
            case 22:
            {
                fieldSize = PlayFieldSize.Large;
                break;
            }
        }
        return new GetSavedPlayFieldResponse(fieldSize, field.EmptyFields);
    }
    
}