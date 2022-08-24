namespace Minesweeper.Server.Entities;

public class Field
{
    public Field(Position position)
    {
        Position = position;
    }

    public Position Position { get; }

    public bool Bomb { get; set; }

    public bool Visible { get; set; }

    public int BombCount { get; set; }

    public bool IsCleared()
    {
        return !Bomb && Visible;
    }

    public bool IsExploded()
    {
        return Bomb && Visible;
    }
}