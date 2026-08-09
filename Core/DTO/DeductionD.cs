namespace Core.DTO
{
    public class DeductionD
    {
        [Required]
        public string CreatedBy { get; set; } = string.Empty;
        [Required]
        public decimal Amount { get; set; }
        [Required]
        [MaxLength(100)]
        public string Reason { get; set; } = string.Empty;
        [Required]
        public string BarberId { get; set; } = string.Empty;
    }
}
