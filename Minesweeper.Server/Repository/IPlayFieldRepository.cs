using Minesweeper.Server.Entities;
using Minesweeper.Server.Enums;

namespace Minesweeper.Server.repository;

public interface IPlayFieldRepository
{
    Guid SetupField(PlayFieldSize playFieldType);

    Field GetField(Guid id, Position position);

    List<ClearedField>? OnClick(Guid id, Field field);
}