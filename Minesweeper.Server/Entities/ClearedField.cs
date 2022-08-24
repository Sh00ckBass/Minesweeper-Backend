namespace Minesweeper.Server.Entities;

public class ClearedField
{
    public Position Position { get; }
    public int BombCount { get;  }

    public ClearedField(Position position, int bombCount)
    {
        Position = position;
        BombCount = bombCount;
    }
}