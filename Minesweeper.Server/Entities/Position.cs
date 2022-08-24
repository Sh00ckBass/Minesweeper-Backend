namespace Minesweeper.Server.Entities;

public class Position
{
    public Position(int x, int y)
    {
        X = x;
        Y = y;
    }

    public int X { get; init; }
    public int Y { get; init; }

    public override bool Equals(object? obj)
    {
        if (obj == null || obj is not Position position) return false;
        return X == position.X && Y == position.Y;
    }

    public override int GetHashCode()
    {
        return X.GetHashCode() ^ Y.GetHashCode();
    }
}