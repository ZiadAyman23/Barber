namespace Core.Models
{
    public class Expense
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public DateOnly Date { get; set; }
        public decimal Amount { get; set; }
        public string Reason { get; set; } = string.Empty;
        public string CreatedBy { get; set; } = string.Empty;
    }
}
