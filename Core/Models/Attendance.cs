namespace Core.Models
{

    public class Attendance
    {
        public enum AttendanceStatus
        {
            Present,
            Absent
        }
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public DateOnly Date { get; set; }
        public AttendanceStatus IsAddent { get; set; } = AttendanceStatus.Absent;
        public string BarberId { get; set; } = string.Empty;
        public Barber? Barber { get; set; }
    }
}
