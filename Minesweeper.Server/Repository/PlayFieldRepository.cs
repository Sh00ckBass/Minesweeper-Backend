using Minesweeper.Server.Entities;
using Minesweeper.Server.Enums;
using Minesweeper.Server.Exceptions;
using Swashbuckle.AspNetCore.Swagger;

namespace Minesweeper.Server.repository;

public class PlayFieldRepository : IPlayFieldRepository
{
    public PlayFieldRepository()
    {
        PlayFields = new Dictionary<Guid, PlayField>();
    }

    private Dictionary<Guid, PlayField> PlayFields { get; }

    public Guid SetupField(PlayFieldSize playFieldType)
    {
        var id = Guid.NewGuid();
        var field = new PlayField(id, playFieldType);
        PlayFields.Add(id, field);
        return id;
    }

    public Field GetField(Guid id, Position position)
    {
        try
        {
            return GetPlayField(id).Fields[position];
        }
        catch (KeyNotFoundException)
        {
            throw new InvalidFieldException();
        }
    }

    public List<ClearedField>? OnClick(Guid id, Field field)
    {
        return GetPlayField(id).OnClick(field);
    }
    
    private PlayField GetPlayField(Guid id)
    {
        try
        {
            return PlayFields[id];
        }
        catch (KeyNotFoundException)
        {
            throw new UnknownPlayFieldException();
        }
    }
}