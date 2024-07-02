using System;
using System.Collections.Generic;

namespace EvoltingStore.Entity
{
    public partial class UserDetail
    {
        public int UserId { get; set; }
        public string FirstName { get; set; } = null!;
        public string LastName { get; set; } = null!;
        public string Email { get; set; } = null!;
        public DateTime CreatedDate { get; set; }
        public string? Image { get; set; }

        public virtual User User { get; set; } = null!;
    }
}
