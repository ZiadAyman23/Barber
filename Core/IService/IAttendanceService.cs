namespace Core.IService
{
    public interface IAttendanceService
    {
        public Task AddAttendance(List<AttendanceD> attendance);
        public List<AttendanceMV> GetAttendanceByDate(DateOnly date);
        public Task<List<AttendanceMV>> GetAttendanceByBarberId(Pagenation pagenation, string barberId);
        public Task UpdateAttendance(AttendanceD attendance, string id);
        public Task<AttendDurationMV> GetDurrationAttendance(Pagenation pagenation, DurationBarberAttend duration);
    }
}
