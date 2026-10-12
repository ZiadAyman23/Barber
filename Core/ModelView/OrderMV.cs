namespace Core.ModelView
{
    public class OrderMV
    {
        public string id { get; set; } = string.Empty;
        public DateTime Date { get; set; }
        public decimal TotalPrice { get; set; }
        public string? ClientName { get; set; }
        public string BarberName { get; set; } = string.Empty;
        public string BarberId { get; set; } = string.Empty;
        public List<ItemOrderMV> Items { get; set; } = new List<ItemOrderMV>();
    }
}
