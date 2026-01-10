using System;
using System.Collections.Generic;

namespace PokerServer.Models;

public partial class RefreshToken
{
    public int Id { get; set; }

    public int PlayerId { get; set; }

    public string TokenHash { get; set; } = null!;

    public DateTime ExpiresAt { get; set; }

    public bool Revoked { get; set; }

    public virtual Player Player { get; set; } = null!;
}
