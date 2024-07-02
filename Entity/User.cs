using System;
using System.Collections.Generic;

namespace EvoltingStore.Entity
{
    public partial class User
    {
        public User()
        {
            Comments = new HashSet<Comment>();
            Orders = new HashSet<Order>();
            Games = new HashSet<Game>();
            Ids = new HashSet<CartItem>();
        }

        public int UserId { get; set; }
        public string Username { get; set; } = null!;
        public string Password { get; set; } = null!;
        public int RoleId { get; set; }
        public bool IsActive { get; set; }
        public int CartId { get; set; }

        public virtual Role Role { get; set; } = null!;
        public virtual UserDetail? UserDetail { get; set; }
        public virtual ICollection<Comment> Comments { get; set; }
        public virtual ICollection<Order> Orders { get; set; }

        public virtual ICollection<Game> Games { get; set; }
        public virtual ICollection<CartItem> Ids { get; set; }
    }
}
