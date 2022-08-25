using Microsoft.AspNetCore.SignalR;
using Minesweeper.Server.Enums;
using Minesweeper.Server.Requests;
using Minesweeper.Server.Responses;
using Minesweeper.Server.Services;

namespace Minesweeper.Server.HubConfig;

public class GameHub : Hub
{
    private readonly IGameService _gameService;

    public GameHub(IGameService gameService)
    {
        _gameService = gameService;
    }

    public async Task StartGame(PlayFieldSize fieldType) =>
        await Clients.All.SendAsync("startGame", _gameService.StartGame(fieldType));

    public async Task RevealField(RevealRequest request) =>
        await Clients.All.SendAsync("revealField", _gameService.RevealField(request));

    public async Task RevealBombs(Guid id) => 
        await Clients.All.SendAsync("revealBombs", new RevealBombsResponse(_gameService.GetPlayField(id).RevealBombs()));
}