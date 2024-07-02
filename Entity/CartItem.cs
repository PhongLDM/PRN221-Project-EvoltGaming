using System;
using System.Collections.Generic;

namespace EvoltingStore.Entity
{
    public partial class CartItem
    {
        public CartItem()
        {
            Users = new HashSet<User>();
        }

        public int Id { get; set; }
        public int CartId { get; set; }
        public int GameId { get; set; }

        public virtual ICollection<User> Users { get; set; }
    }
}
