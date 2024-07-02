namespace EvoltingStore.Entity {
    public class Order {
        /*
         Order:
            orderId
            userId
            totalPrice
            orderDate
            payment
            status
        */
        public Order() {
            OrderDetails = new HashSet<OrderDetail>();
        }

        public int OrderId {  get; set; }
        public int UserId { get; set; }
        public double TotalPrice { get; set; }
        public DateTime OrderDate { get; set; }
        public string? Payment {  get; set; }
        public string? Status {  get; set; }

        public virtual User User { get; set; } = null!;
        public virtual ICollection<OrderDetail> OrderDetails { get; set; }
    }
}
