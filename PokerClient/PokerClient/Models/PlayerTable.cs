using System.ComponentModel.DataAnnotations;

namespace PokerClient.Models;

public class PlayerTable
{
    [Key]
    public Player Player { get; set; }
    
    public Table Table { get; set; }
    
    public int Balance { get; set; }
}