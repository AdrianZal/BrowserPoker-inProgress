using System;
using System.Collections.Generic;

namespace Poker.Models;

public partial class Card
{
    public string Card1 { get; set; } = null!;

    public virtual ICollection<CardFrontSkin> CardFrontSkins { get; set; } = new List<CardFrontSkin>();

    public virtual ICollection<PlayerEquippedFaceSkin> PlayerEquippedFaceSkins { get; set; } = new List<PlayerEquippedFaceSkin>();
}
