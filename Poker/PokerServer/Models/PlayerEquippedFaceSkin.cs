using System;
using System.Collections.Generic;

namespace PokerServer.Models;

public partial class PlayerEquippedFaceSkin
{
    public int PlayerId { get; set; }

    public string Card { get; set; } = null!;

    public int SkinId { get; set; }

    public virtual Card CardNavigation { get; set; } = null!;

    public virtual PlayerOwnedFaceSkin PlayerOwnedFaceSkin { get; set; } = null!;
}
