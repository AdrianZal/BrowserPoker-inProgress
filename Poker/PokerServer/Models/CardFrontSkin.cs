using System;
using System.Collections.Generic;

namespace PokerServer.Models;

public partial class CardFrontSkin
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public string Filename { get; set; } = null!;

    public string Card { get; set; } = null!;

    public virtual Card CardNavigation { get; set; } = null!;

    public virtual ICollection<PlayerOwnedFaceSkin> PlayerOwnedFaceSkins { get; set; } = new List<PlayerOwnedFaceSkin>();
}
