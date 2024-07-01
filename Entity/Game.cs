using System;
using System.Collections.Generic;

namespace EvoltingStore.Entity
{
    public partial class Game
    {
        public Game()
        {
            Comments = new HashSet<Comment>();
            GameRequirements = new HashSet<GameRequirement>();
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
        public string? PirateLink { get; set; }
        public string? OfficialLink { get; set; }
        public string? Image { get; set; }

        public virtual ICollection<Comment> Comments { get; set; }
        public virtual ICollection<GameRequirement> GameRequirements { get; set; }

        public virtual ICollection<Genre> Genres { get; set; }
        public virtual ICollection<User> Users { get; set; }
    }
}
