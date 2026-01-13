using System;
using System.Collections.Generic;

namespace Poker.Models;

public partial class PlayerTable
{
    public int PlayerId { get; set; }

    public string TableJoinCode { get; set; } = null!;

    public int TableBalance { get; set; }

    public virtual Player Player { get; set; } = null!;

    public virtual Table TableJoinCodeNavigation { get; set; } = null!;
}
