namespace Core.DTO
{
    public class DurationBarberAttend
    {
        public string BarberId { get; set; } = string.Empty;
        public DateOnly StartDate { get; set; }
        public DateOnly EndDate { get; set; }
    }
}
