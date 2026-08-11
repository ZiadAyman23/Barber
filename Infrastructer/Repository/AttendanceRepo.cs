using Infrastructer.Context;

namespace Infrastructer.Repository
{
    public class AttendanceRepo : IAttendanceRepo
    {
        private readonly DataContext dataContext;

        public AttendanceRepo(DataContext dataContext)
        {
            this.dataContext = dataContext;
        }

        public int GetAttendanceCount()
        {
            var count = dataContext.Attendances.ToList().Count;
            return count;

        }

        public async Task<int> GetAttendanceCountByBarberId(string barberId, DateOnly startDate)
        {
            var start = startDate;
            var end = DateOnly.FromDateTime(DateTime.Now);
            var count =await dataContext.Attendances.Where(e => e.BarberId == barberId && e.Date >start && e.Date <= end && e.IsAddent == 0).CountAsync();
            return count;
        }

        public async Task<List<Attendance>> GetDurationAttenWithoutPag(GenralDuration duration)
        {
            DateOnly start = DateOnly.FromDateTime(duration.StartDate);
            DateOnly end = DateOnly.FromDateTime(duration.EndDate);
            var attendances =await dataContext.Attendances.Where(a => a.BarberId == duration.BarberId && a.Date >= start && a.Date <= end)
                .OrderByDescending(a => a.Date).AsNoTracking().ToListAsync();
            return attendances;
        }

        public async Task<int> GetMonthelyBarberAttend(GenralDuration genral)
        {
            DateOnly start = DateOnly.FromDateTime(genral.StartDate);
            DateOnly end = DateOnly.FromDateTime(genral.EndDate);

            var count = await dataContext.Attendances.Where(e=>e.IsAddent== 0 && e.BarberId==genral.BarberId && e.Date>=start && e.Date<end ).CountAsync();
            return count;
        }

        async Task IAttendanceRepo.AddAttendance(Attendance attendance)
        {
            var attendanceExists = dataContext.Attendances.Any(a => a.BarberId == attendance.BarberId && a.Date == attendance.Date);

            if (attendanceExists)
            {
                throw new InvalidOperationException($"Attendance for Barber ID {attendance.BarberId} on Date {attendance.Date} already exists.");
            }
            dataContext.Attendances.Add(attendance);
            await dataContext.SaveChangesAsync();
        }

        async Task<Attendance> IAttendanceRepo.GetAttendance(string id)
        {
            var attendance = await dataContext.Attendances.FindAsync(id);
            if (attendance == null)
            {
                throw new KeyNotFoundException($"Attendance with ID {id} not found.");
            }
            return attendance;
        }

        async Task<List<Attendance>> IAttendanceRepo.GetAttendanceByBarberId(Pagenation pagenation, string barberId)
        {
            var attendances = await dataContext.Attendances
                .Where(a => a.BarberId == barberId).OrderByDescending(a => a.Date).Skip((pagenation.PageNumber - 1) * pagenation.PageSize)
                .Take(pagenation.PageSize).ToListAsync();


            return attendances;
        }

        List<Attendance> IAttendanceRepo.GetAttendanceByDate(DateOnly date)
        {
            var attendances = dataContext.Attendances.Include(a => a.Barber)
                .Where(a => a.Date == date).ToList();

            return attendances;
        }

        async Task<(List<Attendance> attendances, string message, int Count)> IAttendanceRepo.GetDurrationAttendance(Pagenation pagenation, DurationBarberAttend duration)
        {
            DateOnly date = duration.StartDate;
            string masseage = string.Empty;
            var barber = dataContext.Barbers.Find(duration.BarberId);
            if (barber == null)
            {
                throw new KeyNotFoundException($"Barber with ID {duration.BarberId} not found.");
            }
            else if (duration.EndDate < barber.StartDate)
            {
                masseage = $"In This Durration {barber.Name} Did not Start , He Start At {barber.StartDate}";
            }
            else if (duration.StartDate < barber.StartDate)
            {
                date = barber.StartDate;
                masseage = $"Start date adjusted to barber's start date: {barber.StartDate}.";
            }


            var attendances = await dataContext.Attendances
                    .Where(a => a.BarberId == duration.BarberId && a.Date >= duration.StartDate && a.Date <= duration.EndDate).OrderByDescending(a => a.Date)
                    .Skip((pagenation.PageNumber - 1) * pagenation.PageSize)
                    .Take(pagenation.PageSize).ToListAsync();
            int count = attendances.Count;

            return (attendances, masseage, count);
        }

        async Task IAttendanceRepo.UpdateAttendance(Attendance attendance)
        {
            dataContext.Attendances.Update(attendance);
            await dataContext.SaveChangesAsync();
        }


    }
}
