using Minesweeper.Server.Entities;
using Minesweeper.Server.Enums;
using Minesweeper.Server.Exceptions;
using Minesweeper.Server.repository;

namespace Minesweeper.Server.Services;

public class GameService : IGameService
{
    private readonly IPlayFieldRepository _playFieldRepository;

    public GameService(IPlayFieldRepository playFieldRepository)
    {
        _playFieldRepository = playFieldRepository;
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
}