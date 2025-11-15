using System.ComponentModel.DataAnnotations;

namespace PokerClient.Models;

public class PlayerOwnedReverseSkin
{
    [Key]
    public Player Player { get; set; }
    
    [Key]
    public CardReverseSkin CardReverseSkin { get; set; }
}