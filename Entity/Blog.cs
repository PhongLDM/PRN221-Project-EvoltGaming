using System;
using System.Collections.Generic;

namespace EvoltingStore.Entity
{
    public partial class Blog
    {
        public int Id { get; set; }
        public string? Title { get; set; }
        public string Content { get; set; } = null!;
        public DateTime? CreateDate { get; set; }
        public int GenreId { get; set; }

        public virtual Genre Genre { get; set; } = null!;
    }
}
