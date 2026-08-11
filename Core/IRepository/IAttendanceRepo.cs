namespace Core.IRepository
{
    public interface IAttendanceRepo
    {
        public Task AddAttendance(Attendance attendance);
        public Task UpdateAttendance(Attendance attendance);
        public Task<Attendance> GetAttendance(string id);
        public List<Attendance> GetAttendanceByDate(DateOnly date);
        public int GetAttendanceCount();
        public Task<int> GetAttendanceCountByBarberId(string barberId, DateOnly startDate);
        public Task<List<Attendance>> GetAttendanceByBarberId(Pagenation pagenation, string barberId);
        public Task<(List<Attendance> attendances, string message, int Count)> GetDurrationAttendance(Pagenation pagenation, DurationBarberAttend duration);
        public Task<List<Attendance>> GetDurationAttenWithoutPag(GenralDuration duration);
        public Task<int> GetMonthelyBarberAttend(GenralDuration genral);
    }
}
