namespace Core.Models
{
    public class Barber
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public string Name { get; set; } = string.Empty;
        public int? HisPercent { get; set; }
        public DateOnly StartDate { get; set; }
        public decimal? DailyPrice { get; set; }
        public List<Order> Orders { get; set; } = new List<Order>();
        public List<Deductions> Deductions { get; set; } = new List<Deductions>();
        public List<Attendance> Attendances { get; set; } = new List<Attendance>();
        public List<Salaries> Salaries { get; set; } = new List<Salaries>();
    }
}
