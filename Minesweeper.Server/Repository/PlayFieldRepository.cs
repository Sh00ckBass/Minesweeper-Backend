using Minesweeper.Server.Entities;
using Minesweeper.Server.Enums;
using Minesweeper.Server.Exceptions;
using MongoDB.Driver;

namespace Minesweeper.Server.repository;

public class PlayFieldRepository : IPlayFieldRepository
{
    private readonly IMongoCollection<PlayField> _playFieldCollection;

    public PlayFieldRepository(IMongoCollection<PlayField> playFieldCollection)
    {
        _playFieldCollection = playFieldCollection;
    }

    public Guid SetupField(PlayFieldSize playFieldType)
    {
        var id = Guid.NewGuid();
        var field = new PlayField(id, playFieldType);
        _playFieldCollection.InsertOne(field);
        return id;
    }

    public Field GetField(PlayField playField, Position position)
    {
        try
        {
            return playField.GetField(position);
        }
        catch (KeyNotFoundException)
        {
            throw new InvalidFieldException();
        }
    }

    public List<ClearedField> OnClick(PlayField playField, Field field)
    {
        return playField.OnClick(field);
    }

    public PlayField GetPlayFieldMongo(Guid id)
    {
        try
        {
            return _playFieldCollection.AsQueryable().First(pf => pf.Id.Equals(id));
        }
        catch (KeyNotFoundException)
        {
            throw new UnknownPlayFieldException();
        }
    }

    public void UpdatePlayField(PlayField playField)
    {
        _playFieldCollection.ReplaceOne(Builders<PlayField>.Filter.Eq(pf => pf.Id, playField.Id), playField);
    }
}