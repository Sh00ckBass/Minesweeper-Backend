using Minesweeper.Server.Entities;
using Minesweeper.Server.Enums;

namespace Minesweeper.Server.Responses;

public class RevealResponse
{
    public RevealResult RevealResult { get; set; }
    public GameState GameState { get; set; }

    public int? BombCount { get; set; }
    public List<ClearedField>? ClearedFields { get; set; }
    
    public Position? Position { get; set; }

    public RevealResponse(RevealResult revealResult, GameState gameState, Position? position)
    {
        RevealResult = revealResult;
        GameState = gameState;
        Position = position;
    }
    
    public RevealResponse(RevealResult revealResult, GameState gameState, int? bombCount, List<ClearedField>? clearedFields, Position? position)
    {
        RevealResult = revealResult;
        GameState = gameState;
        BombCount = bombCount;
        ClearedFields = clearedFields;
        Position = position;
    }
}