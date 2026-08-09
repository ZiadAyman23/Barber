namespace Core.Models
{
    public class Deductions
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public DateTime Date { get; set; }
        public string CreatedBy { get; set; } = string.Empty;
        public decimal Amount { get; set; }
        public string Reason { get; set; } = string.Empty;
        public string BarberId { get; set; } = string.Empty;
        public Barber? Barber { get; set; }
    }
}
