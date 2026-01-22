using Microsoft.AspNetCore.SignalR;
using Poker.Game;
using PokerServer.Hubs;

public class GameService(IHubContext<PokerHub> hubContext)
{
    private readonly Dictionary<string, Table> _tables = new();

    public string CreateTable(int buyIn)
    {
        string code = Guid.NewGuid().ToString("N").Substring(0, 6).ToUpper();
        var table = new Table(buyIn ,code);

        table.OnGameStateChanged += async (state) =>
        {
            await hubContext.Clients.Group(code).SendAsync("UpdateState", state);
        };

        _tables.Add(code, table);
        return code;
    }

    public Table? GetTable(string code) => _tables.GetValueOrDefault(code);
}