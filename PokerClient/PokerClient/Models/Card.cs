using System.ComponentModel.DataAnnotations;

namespace PokerClient.Models;

public class Card
{
    [Key]
    public int Id { get; set; }
    
    public string Name { get; set; }
}