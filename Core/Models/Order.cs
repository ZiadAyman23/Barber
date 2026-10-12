namespace Core.Models
{
    public class Order
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public DateTime Date { get; set; }
        public decimal TotalPrice { get; set; } = 0;
        public string? ClientName { get; set; } = string.Empty;
        public string BarberId { get; set; } = string.Empty;
        public Barber? Barber { get; set; }
        public List<ItemOrder> Items { get; set; } = new List<ItemOrder>();
    }
}
