using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PokerClient.Models;

public class Table
{
    [Key]
    [Column(TypeName = "varchar(6)")]
    public string JoinCode { get; set; }
    
    public int BuyIn { get; set; }
}