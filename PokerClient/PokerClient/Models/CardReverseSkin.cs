using System.ComponentModel.DataAnnotations;

namespace PokerClient.Models;

public class CardReverseSkin
{
    public int Id { get; set; }
    
    [MaxLength(50)]
    public string Name { get; set; }
    
    [MaxLength(54)]
    public string File { get; set; }
}