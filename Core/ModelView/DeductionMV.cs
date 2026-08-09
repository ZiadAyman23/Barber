namespace Core.ModelView
{
    public class DeductionMV
    {
        public string id { get; set; } = string.Empty;
        public DateTime DateOnly { get; set; }
        public string CreatedBy { get; set; } = string.Empty;
        public decimal Amount { get; set; }
        public string Reason { get; set; } = string.Empty;
        public string BaeberName { get; set; } = string.Empty;
        public string BarberId { get; set; } = string.Empty;
    }
}
