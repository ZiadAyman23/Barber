namespace Core.DTO
{
    public class ExpenseD
    {
        [Required]
        public decimal Amount { get; set; }
        [Required]
        public string Reason { get; set; } = string.Empty;
        [Required]
        public string CreatedBy { get; set; } = string.Empty;
    }
}
