using Minesweeper.Server.Entities;

namespace Minesweeper.Server.Responses;

public class RevealBombsResponse
{
    public List<Position> bombs { get; }

    public RevealBombsResponse(List<Position> bombs)
    {
        this.bombs = bombs;
    }
}