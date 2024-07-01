using System;
using System.Collections.Generic;

namespace EvoltingStore.Entity
{
    public partial class Genre
    {
        public Genre()
        {
            Games = new HashSet<Game>();
        }

        public int GenreId { get; set; }
        public string GenreName { get; set; } = null!;

        public virtual ICollection<Game> Games { get; set; }
    }
}
