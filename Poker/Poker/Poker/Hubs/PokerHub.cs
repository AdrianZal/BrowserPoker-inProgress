using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using Poker.Models;


namespace PokerServer.Hubs
{
    [Authorize]
    public class PokerHub(PokerContext context, GameService gameService) : Hub
    {
        public async Task JoinTable(string joinCode)
        {
            var userIdClaim = Context.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (string.IsNullOrEmpty(userIdClaim) || !int.TryParse(userIdClaim, out int playerId))
            {
                throw new HubException("User identity not found.");
            }

            var playerInfo = await context.Players.FindAsync(playerId);

            var table = gameService.GetTable(joinCode);
            if (table == null) throw new HubException("Table not found.");

            if(table.buyIn > playerInfo.Balance)
                throw new HubException("Insufficient balance to join the table.");
            var player = new Poker.Game.Player (playerInfo.Name, table.buyIn);
            table.AddPlayer(player);

            await Groups.AddToGroupAsync(Context.ConnectionId, joinCode);

            await Clients.Group(joinCode).SendAsync("PlayerJoined", player.name);
        }

        public async Task SendAction(string joinCode, int amount)
        {
            var table = gameService.GetTable(joinCode);
        }
    }
}
