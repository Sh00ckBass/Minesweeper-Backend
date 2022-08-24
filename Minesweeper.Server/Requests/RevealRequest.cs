using Minesweeper.Server.Entities;

namespace Minesweeper.Server.Requests;

public class RevealRequest
{
    public Guid PlayFieldId { get; set; }
    public Position Position { get; set; }
}