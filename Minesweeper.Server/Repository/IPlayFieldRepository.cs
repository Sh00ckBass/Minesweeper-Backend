using Minesweeper.Server.Entities;
using Minesweeper.Server.Enums;

namespace Minesweeper.Server.repository;

public interface IPlayFieldRepository
{
    Guid SetupField(PlayFieldSize playFieldType);

    Field GetField(PlayField playField, Position position);
    PlayField GetPlayFieldMongo(Guid id);

    List<ClearedField> OnClick(PlayField playField, Field field);

    void UpdatePlayField(PlayField playField);
}