using System.ComponentModel.DataAnnotations;

namespace PokerClient.Models;

public class Player
{
    public int Id { get; set; }
    
    [MaxLength(30)]
    public string Name { get; set; }
    
    public int Balance { get; set; }
    
    [MaxLength(64)]
    public string Salt { get; set; }
    
    [MaxLength(64)]
    public string Password { get; set; }
}