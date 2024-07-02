using System;
using System.Collections.Generic;

namespace EvoltingStore.Entity
{
    public partial class OrderDetail
    {
        public int Id { get; set; }
        public int OrderId { get; set; }
        public int GameId { get; set; }

        public virtual Game Game { get; set; } = null!;
        public virtual Order Order { get; set; } = null!;
    }
}
