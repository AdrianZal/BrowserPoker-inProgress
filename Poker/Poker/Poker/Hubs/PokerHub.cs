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

                playerInfo.Balance -= table.buyIn;
                try
                {
                    await context.SaveChangesAsync();
                }
                catch
                {
                    throw new HubException("Transaction failed.");
                }

                var player = new Poker.Game.Player(playerInfo.Name, table.buyIn);
                table.AddPlayer(player);

                await Clients.Group(joinCode).SendAsync("PlayerJoined", player.name);
            }

            await Groups.AddToGroupAsync(Context.ConnectionId, joinCode);
        }

        public async Task LeaveTable(string joinCode)
        {
            var userIdClaim = Context.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userIdClaim) || !int.TryParse(userIdClaim, out int playerId))
            {
                throw new HubException("User identity not found.");
            }

            var table = gameService.GetTable(joinCode);
            if (table == null)
            {
                await Groups.RemoveFromGroupAsync(Context.ConnectionId, joinCode);
                return;
            }

            var playerInfo = await context.Players.FindAsync(playerId);
            if (playerInfo == null) throw new HubException("Player profile not found.");

            var tablePlayer = table.players.FirstOrDefault(p => p.name == playerInfo.Name);

            if (tablePlayer != null)
            {
                playerInfo.Balance += tablePlayer.tableBalance;

                table.RemovePlayer(tablePlayer);

                await context.SaveChangesAsync();

                await Clients.Group(joinCode).SendAsync("PlayerLeft", tablePlayer.name);

                table.NotifyStateUpdate();
            }

            await Groups.RemoveFromGroupAsync(Context.ConnectionId, joinCode);
        }

        public override async Task OnDisconnectedAsync(Exception? exception)
        {
            var userIdClaim = Context.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (!string.IsNullOrEmpty(userIdClaim) && int.TryParse(userIdClaim, out int playerId))
            {
                var playerInfo = await context.Players.FindAsync(playerId);
                if (playerInfo != null)
                {
                    var table = gameService.GetTableByPlayer(playerInfo.Name);

                    if (table != null)
                    {
                        var tablePlayer = table.players.FirstOrDefault(p => p.name == playerInfo.Name);
                        if (tablePlayer != null)
                        {
                            playerInfo.Balance += tablePlayer.tableBalance;
                            table.RemovePlayer(tablePlayer);
                            await context.SaveChangesAsync();

                            await Clients.Group(table.joinCode).SendAsync("PlayerLeft", tablePlayer.name);
                            table.NotifyStateUpdate();
                        }
                    }
                }
            }

            await base.OnDisconnectedAsync(exception);
        }

        public async Task SendAction(string joinCode, int amount)
        {
            var table = gameService.GetTable(joinCode);
        }
    }
}
