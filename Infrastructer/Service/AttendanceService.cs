using static Core.Models.Attendance;

namespace Infrastructer.Service
{
    public class AttendanceService : IAttendanceService
    {
        private readonly IAttendanceRepo attendanceRepo;
        private readonly IBaeberRepo baeberRepo;

        public AttendanceService(IAttendanceRepo attendanceRepo, IBaeberRepo baeberRepo)
        {
            this.attendanceRepo = attendanceRepo;
            this.baeberRepo = baeberRepo;
        }

        async Task IAttendanceService.AddAttendance(List<AttendanceD> attendances)
        {
            foreach (var attendance in attendances)
            {

                Attendance attendance1 = new Attendance();
                if (attendance.IsPresent == true)
                {
                    attendance1.IsAddent = AttendanceStatus.Present;
                }
                else
                {
                    attendance1.IsAddent = AttendanceStatus.Absent;
                }
                attendance1.Date = attendance.DateOnly;
                attendance1.BarberId = attendance.BarberId;
                await attendanceRepo.AddAttendance(attendance1);
            }
        }

        async Task<List<AttendanceMV>> IAttendanceService.GetAttendanceByBarberId(Pagenation pagenation, string barberId)
        {
            if (pagenation.PageNumber < 1)
                pagenation.PageNumber = 1;

            if (pagenation.PageSize < 1)
                pagenation.PageSize = 1;

            var attendances = await attendanceRepo.GetAttendanceByBarberId(pagenation, barberId);

            List<AttendanceMV> attendanceMVList = new List<AttendanceMV>();
            foreach (var attendance in attendances)
            {
                AttendanceMV attendanceMV = new AttendanceMV
                {
                    AttendanceId = attendance.Id,
                    BarberId = attendance.BarberId,
                    Status = attendance.IsAddent.ToString(),
                    Date = attendance.Date
                };
                attendanceMVList.Add(attendanceMV);
            }
            return attendanceMVList;
        }

        List<AttendanceMV> IAttendanceService.GetAttendanceByDate(DateOnly date)
        {

            var attendances = attendanceRepo.GetAttendanceByDate(date);
            List<AttendanceMV> attendanceMVList = new List<AttendanceMV>();
            foreach (var attendance in attendances)
            {
                AttendanceMV attendanceMV = new AttendanceMV
                {
                    AttendanceId = attendance.Id,
                    BarberId = attendance.BarberId,
                    BarberName = attendance.Barber.Name,
                    Status = attendance.IsAddent.ToString(),
                    Date = attendance.Date
                };
                attendanceMVList.Add(attendanceMV);
            }
            return attendanceMVList;
        }


        async Task<AttendDurationMV> IAttendanceService.GetDurrationAttendance(Pagenation pagenation, DurationBarberAttend duration)
        {

            if (pagenation.PageNumber < 1)
                pagenation.PageNumber = 1;

            if (pagenation.PageSize < 1)
                pagenation.PageSize = 1;

            var attend = await attendanceRepo.GetDurrationAttendance(pagenation, duration);

            AttendDurationMV attendDurationMV = new AttendDurationMV();


            foreach (var attendance in attend.attendances)
            {
                AttendanceMV attendanceMV = new AttendanceMV
                {
                    AttendanceId = attendance.Id,
                    BarberId = attendance.BarberId,

                    Status = attendance.IsAddent.ToString(),
                    Date = attendance.Date
                };
                attendDurationMV.attendanceMVs.Add(attendanceMV);
            }


            attendDurationMV.Massage = attend.message;


            return attendDurationMV;
        }

        async Task IAttendanceService.UpdateAttendance(AttendanceD attendance, string id)
        {

            var attendanceToUpdate = await attendanceRepo.GetAttendance(id);
            if (attendanceToUpdate == null)
                throw new KeyNotFoundException($"Attendance with ID {id} not found.");

            if (attendance.IsPresent == true)
            {
                attendanceToUpdate.IsAddent = AttendanceStatus.Present;
            }
            else
            {
                attendanceToUpdate.IsAddent = AttendanceStatus.Absent;
            }
            await attendanceRepo.UpdateAttendance(attendanceToUpdate);

        }
    }
}
