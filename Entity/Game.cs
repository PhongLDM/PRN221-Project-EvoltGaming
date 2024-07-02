using System;
using System.Collections.Generic;

namespace EvoltingStore.Entity
{
    public partial class Game
    {
        public Game()
        {
            CartItems = new HashSet<CartItem>();
            Comments = new HashSet<Comment>();
            GameRequirements = new HashSet<GameRequirement>();
            OrderDetails = new HashSet<OrderDetail>();
            Genres = new HashSet<Genre>();
            Users = new HashSet<User>();
        }

        public int GameId { get; set; }
        public string Name { get; set; } = null!;
        public string Description { get; set; } = null!;
        public string Publisher { get; set; } = null!;
        public DateTime ReleaseDate { get; set; }
        public DateTime UpdateDate { get; set; }
        public string Platform { get; set; } = null!;
        public string? Image { get; set; }
        public double Price { get; set; }

        public virtual ICollection<CartItem> CartItems { get; set; }
        public virtual ICollection<Comment> Comments { get; set; }
        public virtual ICollection<GameRequirement> GameRequirements { get; set; }
        public virtual ICollection<OrderDetail> OrderDetails { get; set; }

        public virtual ICollection<Genre> Genres { get; set; }
        public virtual ICollection<User> Users { get; set; }
    }
}
