using System;
using System.Collections.Generic;

namespace EvoltingStore.Entity
{
    public partial class CartItem
    {
        public int Id { get; set; }
        public int CartId { get; set; }
        public int GameId { get; set; }

        public virtual Cart Cart { get; set; } = null!;
        public virtual Game Game { get; set; } = null!;
    }
}
