using System;
using System.Collections.Generic;

namespace Poker.Models;

public partial class CardReverseSkin
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public string Filename { get; set; } = null!;

    public virtual ICollection<PlayerOwnedReverseSkin> PlayerOwnedReverseSkins { get; set; } = new List<PlayerOwnedReverseSkin>();
}
