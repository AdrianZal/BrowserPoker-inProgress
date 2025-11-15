using Microsoft.EntityFrameworkCore;

namespace PokerClient.Models;

public class DBContext : DbContext
{
    public DBContext()
    {
        
    }

    public DBContext(DbContextOptions<DBContext> options) : base(options){}
    
    public DbSet<Table> Tables { get; set; }
    
    public DbSet<Player> Players { get; set; }
    
    public DbSet<Card> Cards { get; set; }
    
    public DbSet<PlayerTable> PlayerTables { get; set; }
    
    public DbSet<CardReverseSkin>  CardReverseSkins { get; set; }
    
    public DbSet<PlayerOwnedReverseSkin> PlayerOwnedReverseSkins{ get; set; }
    
    public DbSet<PlayerEquippedReverseSkin>  PlayerEquippedReverseSkins{ get; set; }
    
    public DbSet<CardFrontSkin>  CardFrontSkins { get; set; }
    
    public DbSet<PlayerOwnedFrontSkin> PlayerOwnedFrontSkins{ get; set; }
    
    public DbSet<PlayerEquippedFrontSkin>   PlayerEquippedFrontSkins{ get; set; }
}