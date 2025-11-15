using System.ComponentModel.DataAnnotations;

namespace PokerClient.Models;

public class PlayerOwnedFrontSkin
{
    [Key]
    public Player Player { get; set; }
    
    [Key]
    public CardFrontSkin CardFrontSkin { get; set; }
}