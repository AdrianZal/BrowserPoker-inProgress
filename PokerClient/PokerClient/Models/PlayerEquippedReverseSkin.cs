using System.ComponentModel.DataAnnotations;

namespace PokerClient.Models;

public class PlayerEquippedReverseSkin
{
    [Key]
    public Player Player { get; set; }
    
    public CardReverseSkin CardReverseSkin { get; set; }
}