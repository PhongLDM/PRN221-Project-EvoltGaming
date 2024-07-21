using System;
using System.Collections.Generic;

namespace EvoltingStore.Entity
{
    public partial class Blog
    {
        public int Id { get; set; }
        public string? Title { get; set; }
        public string Content { get; set; } = ""!;
        public DateTime? CreateDate { get; set; } = DateTime.Now;
        public int GenreId { get; set; }

        public virtual Genre Genre { get; set; } = null!;
    }
}
