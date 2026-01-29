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
            if (playerInfo == null) throw new HubException("Player profile not found.");

            var table = gameService.GetTable(joinCode);
            if (table == null) throw new HubException("Table not found.");

            var existingPlayer = table.players.FirstOrDefault(p => p.name == playerInfo.Name);

            if (existingPlayer == null)
            {
                if (table.IsFull()) throw new HubException("Table is full.");

                if (table.buyIn > playerInfo.Balance)
                    throw new HubException("Insufficient balance to join the table.");

                var player = new Poker.Game.Player(playerInfo.Name, table.buyIn);
                table.AddPlayer(player);

                await Clients.Group(joinCode).SendAsync("PlayerJoined", player.name);
            }

            await Groups.AddToGroupAsync(Context.ConnectionId, joinCode);
        }

        public async Task SendAction(string joinCode, int amount)
        {
            var table = gameService.GetTable(joinCode);
        }
    }
}
