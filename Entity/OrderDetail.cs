namespace EvoltingStore.Entity {
    public class OrderDetail {
        public int OrderDetailId { get; set; }
        public int OrderId { get; set; }
        public int GameId { get; set; }

        public virtual Order Order { get; set; } = null!;
        public virtual Game Game { get; set; } = null!;
    }
}
