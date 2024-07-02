using System;
using System.Collections.Generic;

namespace EvoltingStore.Entity
{
    public partial class Order
    {
        public Order()
        {
            OrderDetails = new HashSet<OrderDetail>();
        }

        public int OrderId { get; set; }
        public double TotalPrice { get; set; }
        public DateTime OrderDate { get; set; }
        public bool Status { get; set; }
        public int UserId { get; set; }

        public virtual User User { get; set; }
        public virtual ICollection<OrderDetail> OrderDetails { get; set; }
    }
}
