using System;
using System.Collections.Generic;

namespace EvoltingStore.Entity
{
    public partial class Favourite
    {
        public int GameId { get; set; }
        public int UserId { get; set; }

        public virtual Game Game { get; set; } = null!;

        public virtual User User { get; set; } = null!;
    }
}
