namespace Core.DTO
{
    public class BarberD
    {
        [Required]
        public string Name { get; set; } = string.Empty;
        public int? HisPercent { get; set; }
        public decimal? DailyPrice { get; set; }
    }
}
