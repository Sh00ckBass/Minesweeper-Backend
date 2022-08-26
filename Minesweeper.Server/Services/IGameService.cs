using Minesweeper.Server.Entities;
using Minesweeper.Server.Enums;
using Minesweeper.Server.Requests;
using Minesweeper.Server.Responses;

namespace Minesweeper.Server.Services;

public interface IGameService
{
    Guid StartGame(PlayFieldSize fieldType);
    PlayField GetPlayFieldMongo(Guid id);

    RevealBombsResponse RevealBombs(PlayField field);
    
    RevealResponse RevealField(RevealRequest request);

    GetSavedPlayFieldResponse GetSavedPlayField(Guid playFieldId);
}