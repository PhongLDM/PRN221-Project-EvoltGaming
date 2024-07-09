using System;
using System.Collections.Generic;

namespace EvoltingStore.Entity
{
    public partial class User
    {
        public User()
        {
            Carts = new HashSet<Cart>();
            Comments = new HashSet<Comment>();
            Orders = new HashSet<Order>();
            Games = new HashSet<Game>();
        }

        public int UserId { get; set; }
        public string Username { get; set; } = null!;
        public string Password { get; set; } = null!;
        public int RoleId { get; set; }
        public bool IsActive { get; set; }
        public int? CartId { get; set; }
        public string? ResetToken { get; set; }
        public DateTime? ResetTokenExpiry { get; set; }

        public virtual Cart? Cart { get; set; }
        public virtual Role Role { get; set; } = null!;
        public virtual UserDetail? UserDetail { get; set; }
        public virtual ICollection<Cart> Carts { get; set; }
        public virtual ICollection<Comment> Comments { get; set; }
        public virtual ICollection<Order> Orders { get; set; }
        public virtual ICollection<Game> Games { get; set; }
    }
}
