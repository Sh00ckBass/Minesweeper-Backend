using Minesweeper.Server.Entities;
using Minesweeper.Server.Enums;

namespace Minesweeper.Server.Responses;

public class GetSavedPlayFieldResponse
{

    public PlayFieldSize PlayFieldSize { get; set; }
    public List<Field> EmptyFields { get; set; }

    public GetSavedPlayFieldResponse(PlayFieldSize playFieldSize, List<Field> emptyFields)
    {
        PlayFieldSize = playFieldSize;
        EmptyFields = emptyFields;
    }
}