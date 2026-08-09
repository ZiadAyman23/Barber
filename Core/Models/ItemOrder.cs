namespace Core.Models
{
    public class ItemOrder
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public string Name { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public string OrderId { get; set; } = string.Empty;
        public Order? Order { get; set; }
    }
}
