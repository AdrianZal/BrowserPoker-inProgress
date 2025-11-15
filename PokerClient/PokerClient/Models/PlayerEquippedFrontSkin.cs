using System.ComponentModel.DataAnnotations;

namespace PokerClient.Models;

public class PlayerEquippedFrontSkin
{
    [Key]
    public Player Player { get; set; }
    
    [Key]
    public Card Card { get; set; }
    
    public CardFrontSkin CardFrontSkin { get; set; }
}