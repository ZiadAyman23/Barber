using Core.ModelView;

namespace BarberFront.Services
{
    public class AttentanceService
    {
        private readonly IHttpClientFactory httpClientFactory;
        private readonly AccountService accountService;

        public AttentanceService(IHttpClientFactory httpClientFactory ,AccountService accountService)
        {
            this.httpClientFactory = httpClientFactory;
            this.accountService = accountService;
        }

        public async Task<(List<AttendanceMV> attendances, APIRespons apiRespons)> GetAttendanceByDate(DateOnly date)
        {
            APIRespons aPIRespons = new APIRespons();
            try
            {
                var client = httpClientFactory.CreateClient("API");
                var response = await client.GetAsync($"api/Attendance/GetAttendanceByDate?date={date}");
                List<AttendanceMV> attendances = new List<AttendanceMV>();

                if (response.IsSuccessStatusCode)
                {
                    attendances = await response.Content.ReadFromJsonAsync<List<AttendanceMV>>() ?? new List<AttendanceMV>();
                    aPIRespons.Status = true;
                    aPIRespons.StatusMessage = string.Empty;
                    return (attendances, aPIRespons);
                }
                else
                {
                    aPIRespons.StatusMessage = await response.Content.ReadAsStringAsync();
                    aPIRespons.Status = false;
                    return (attendances, aPIRespons);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                aPIRespons.Status = false;
                aPIRespons.StatusMessage = ex.Message;
                return (new List<AttendanceMV>(), aPIRespons);
            }
        }


        public async Task<APIRespons> AddAttendance(List<AttendanceD> attendance)
        {
            APIRespons aPIRespons = new APIRespons();
            try
            {
                var client = httpClientFactory.CreateClient("API");
                var response = await client.PostAsJsonAsync("api/Attendance/RecordAttendance", attendance);

                if (response.IsSuccessStatusCode)
                {
                    aPIRespons.StatusMessage = "Attendance recorded successfully.";
                    aPIRespons.Status = true;
                    return aPIRespons;
                }
                else
                {
                    aPIRespons.StatusMessage = await response.Content.ReadAsStringAsync();
                    aPIRespons.Status = false;
                    return aPIRespons;
                }
            }
            catch (Exception ex)
            {

                Console.WriteLine(ex.Message);
                aPIRespons.Status = false;
                aPIRespons.StatusMessage = "can not connect to the server";
                return aPIRespons;


            }
        }

        public async Task<List<AttendanceMV>> GatAttendByBarberId(string barberId, Pagenation pagenation)
        {
            try
            {
                var client = httpClientFactory.CreateClient("API");
                var response = await client.GetAsync($"api/Attendance/getBarberAttend/{barberId}?PageNumber={pagenation.PageNumber}&PageSize={pagenation.PageSize}");

                List<AttendanceMV> attendanceMVs = new List<AttendanceMV>();

                if (response.IsSuccessStatusCode)
                {
                    attendanceMVs = await response.Content.ReadFromJsonAsync<List<AttendanceMV>>() ?? new List<AttendanceMV>();


                    return attendanceMVs;
                }
                else
                {
                    var err = await response.Content.ReadAsStringAsync();
                    throw new Exception(err);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<AttendDurationMV> GatDurationAttend(Pagenation pagenation, DurationBarberAttend durationBarberAttend)
        {
            try
            {
                var client = httpClientFactory.CreateClient("API");
                var respons = await client.GetAsync($"api/Attendance/getDurationAttend?PageNumber={pagenation.PageNumber}&PageSize={pagenation.PageSize}&StartDate={durationBarberAttend.StartDate}&EndDate={durationBarberAttend.EndDate}&BarberId={durationBarberAttend.BarberId}");
                string massage = string.Empty;
                AttendDurationMV attendDurationMV = new AttendDurationMV();

                if (respons.IsSuccessStatusCode)
                {
                    attendDurationMV = await respons.Content.ReadFromJsonAsync<AttendDurationMV>() ?? new AttendDurationMV();


                    return attendDurationMV;
                }
                else
                {
                    throw new Exception("");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return new AttendDurationMV();
            }
        }

        public async Task<APIRespons> UpdateAttend(AttendanceD attendanceD, string id,CheckAccess checkAccess)
        {
            APIRespons aPIRespons = new APIRespons();
            try
            {
               var res =await accountService.CheckAccess(checkAccess);
                if (!res.Status)
                { 
                    return res;
                }

                var client = httpClientFactory.CreateClient("API");
                var respons = await client.PutAsJsonAsync($"api/Attendance/editAttend/{id}", attendanceD);
                if (respons.IsSuccessStatusCode)
                {
                    aPIRespons.Status = true;
                    aPIRespons.StatusMessage = "تم تحديث الحضور بنجاح";
                    return aPIRespons;
                }
                else
                {
                    aPIRespons.Status = false;
                    aPIRespons.StatusMessage = "فشل تحديث الحضور";
                    return aPIRespons;
                }
            }
            catch (Exception e)
            {
                aPIRespons.Status = false;
                aPIRespons.StatusMessage = e.Message;
                return aPIRespons;
            }
        }
    }
}
