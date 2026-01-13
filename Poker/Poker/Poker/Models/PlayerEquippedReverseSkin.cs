using System;
using System.Collections.Generic;

namespace Poker.Models;

public partial class PlayerEquippedReverseSkin
{
    public int PlayerId { get; set; }

    public int SkinId { get; set; }

    public virtual PlayerOwnedReverseSkin PlayerOwnedReverseSkin { get; set; } = null!;
}
