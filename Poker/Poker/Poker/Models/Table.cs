using System;
using System.Collections.Generic;

namespace Poker.Models;

public partial class Table
{
    public string JoinCode { get; set; } = null!;

    public int BuyIn { get; set; }

    public virtual ICollection<PlayerTable> PlayerTables { get; set; } = new List<PlayerTable>();
}
