namespace Core.DTO
{
    public class OrderD
    {
        public decimal TotalPrice { get; set; }
        public string BarberId { get; set; } = string.Empty;

        public List<OrderItemD> orderItemDs { get; set; } = new List<OrderItemD>();

    }
}
