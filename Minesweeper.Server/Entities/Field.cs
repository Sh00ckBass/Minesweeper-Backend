namespace Minesweeper.Server.Entities;

public class Field
{
    public Field(Position position)
    {
        Position = position;
    }

    public Position Position { get; set; }

    public bool Bomb { get; set; }

    public bool Visible { get; set; }

    public int BombCount { get; set; }
    
}