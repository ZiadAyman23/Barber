namespace Core.ModelView
{
    public class AttendanceMV
    {
        public string AttendanceId { get; set; } = string.Empty;
        public string BarberName { get; set; } = string.Empty;
        public string BarberId { get; set; } = string.Empty;
        public int AttendCount { get; set; }
        public DateOnly Date { get; set; }
        public string Status { get; set; } = string.Empty;

    }
}
