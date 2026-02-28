using Microsoft.EntityFrameworkCore;
using Poker.Models;
using Poker.Models.DTOs;

namespace Poker.Services
{
    public class UtilService(PokerContext context)
    {
        public async Task<OwnedSkinsDTO> GetOwnedSkinsAsync(string playerName)
        {
            var playerId = await context.Players
                .Where(p => p.Name == playerName)
                .Select(p => p.Id)
                .FirstOrDefaultAsync();

            var ownedFileNames = await context.PlayerOwnedReverseSkins
                .Where(pos => pos.PlayerId == playerId)
                .Select(x => x.Skin.Filename)
                .ToListAsync();

            var equippedSkin = await context.PlayerEquippedReverseSkins
                .Where(x => x.PlayerId == playerId)
                .Select(x => x.PlayerOwnedReverseSkin.Skin.Filename)
                .FirstOrDefaultAsync();

            return new OwnedSkinsDTO
            {
                Skins = ownedFileNames.ToDictionary
                (
                    filename => filename,
                    filename => filename == equippedSkin
                )
            };
        }

        public async Task ChangeEquippedSkinAsync(string playerName, string fileName)
        {
            var ownedSkin = await context.PlayerOwnedReverseSkins
                .Where(os => os.Player.Name == playerName && os.Skin.Filename == fileName)
                .Select(os => new { os.PlayerId, os.SkinId })
                .FirstOrDefaultAsync();

            if (ownedSkin == null) return;

            var currentEquipped = await context.PlayerEquippedReverseSkins
                .FirstOrDefaultAsync(e => e.PlayerId == ownedSkin.PlayerId);

            if (currentEquipped != null)
            {
                currentEquipped.SkinId = ownedSkin.SkinId;
            }
            else
            {
                return;
            }

            await context.SaveChangesAsync();
        }

        public async Task<int> GetBalanceAsync(string playerName)
        {
            var playerBalance = await context.Players
                .Where(p => p.Name == playerName)
                .Select(p => p.Balance)
                .FirstOrDefaultAsync();

            return playerBalance;
        }
    }
}
