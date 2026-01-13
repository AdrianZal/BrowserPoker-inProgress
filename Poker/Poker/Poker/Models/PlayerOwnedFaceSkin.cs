using System;
using System.Collections.Generic;

namespace Poker.Models;

public partial class PlayerOwnedFaceSkin
{
    public int PlayerId { get; set; }

    public int SkinId { get; set; }

    public virtual Player Player { get; set; } = null!;

    public virtual ICollection<PlayerEquippedFaceSkin> PlayerEquippedFaceSkins { get; set; } = new List<PlayerEquippedFaceSkin>();

    public virtual CardFrontSkin Skin { get; set; } = null!;
}
