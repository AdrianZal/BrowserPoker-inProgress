using System;
using System.Collections.Generic;

namespace Poker.Models;

public partial class PlayerOwnedReverseSkin
{
    public int PlayerId { get; set; }

    public int SkinId { get; set; }

    public virtual Player Player { get; set; } = null!;

    public virtual ICollection<PlayerEquippedReverseSkin> PlayerEquippedReverseSkins { get; set; } = new List<PlayerEquippedReverseSkin>();

    public virtual CardReverseSkin Skin { get; set; } = null!;
}
