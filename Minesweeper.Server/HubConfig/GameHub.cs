using Microsoft.AspNetCore.SignalR;
using Minesweeper.Server.Enums;
using Minesweeper.Server.Requests;
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
        await Clients.Client(Context.ConnectionId).SendAsync("startGame", _gameService.StartGame(fieldType));

    public async Task RevealField(RevealRequest request) =>
        await Clients.Client(Context.ConnectionId).SendAsync("revealField", _gameService.RevealField(request));

    public async Task RevealBombs(Guid id) =>
        await Clients.Client(Context.ConnectionId).SendAsync("revealBombs", _gameService.RevealBombs(_gameService.GetPlayFieldMongo(id)));

    public async Task GetPlayField(Guid id) =>
        await Clients.Client(Context.ConnectionId).SendAsync("getPlayField", _gameService.GetSavedPlayField(id));
}