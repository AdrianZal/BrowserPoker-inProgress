using System.ComponentModel.DataAnnotations;

namespace PokerClient.Models;

public class CardFrontSkin
{
    public int Id { get; set; }
    
    [MaxLength(50)]
    public string Name { get; set; }
    
    [MaxLength(3)]
    public string Card { get; set; }
    
    [MaxLength(54)]
    public string File { get; set; }
}