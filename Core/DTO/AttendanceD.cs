namespace Core.DTO
{
    public class AttendanceD
    {
        public string BarberId { get; set; } = string.Empty;
        public bool IsPresent { get; set; }
        public DateOnly DateOnly { get; set; }
    }
}
