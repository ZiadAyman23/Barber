namespace Core.ModelView
{
    public class BarberMV
    {
        public string Id { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public int? HisPercent { get; set; }
        public decimal? DailyPrice { get; set; }
        public int NumberOfOrders { get; set; }
        public int NumberOfDeductions { get; set; }
        public int NumberOfAttendances { get; set; }
        public DateOnly StartDate { get; set; }
        public string sAbsentToday { get; set; } = string.Empty;
        public bool IsAbsentToday { get; set; }
    }
}
