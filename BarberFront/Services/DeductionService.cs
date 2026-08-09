using Core.ModelView;

namespace BarberFront.Services
{
    public class DeductionService
    {
        private readonly IHttpClientFactory httpClientFactory1;
        private readonly AccountService accountService;
        public DeductionService(IHttpClientFactory httpClientFactory, AccountService accountService)
        {
            this.httpClientFactory1 = httpClientFactory;
            this.accountService = accountService;
        }

        public async Task<APIRespons> AddDeductionAsync(DeductionD deductionD)
        {
            var client = httpClientFactory1.CreateClient("API");

            var response = await client.PostAsJsonAsync("api/Deduction/addDeduction", deductionD);

            if (response.IsSuccessStatusCode)
            {
                return new APIRespons { Status = true, StatusMessage = "تم إضافة الخصم بنجاح" };
            }
            else
            {

                return new APIRespons { Status = false, StatusMessage = "فشل إضافة الخصم" };
            }

        }

        public async Task<APIRespons> EditDeduction(DeductionD deductionD, string id, CheckAccess pass)
        {
            var check = await accountService.CheckAccess(pass);
            if (!check.Status)
            {
                return check;
            }

            var client = httpClientFactory1.CreateClient("API");

            var respons = await client.PutAsJsonAsync($"api/Deduction/editDeduction/{id}", deductionD);
            if (respons.IsSuccessStatusCode)
            {
                return new APIRespons { Status = true, StatusMessage = "تم تعديل الخصم بنجاح" };
            }
            else
            {
                var error = await respons.Content.ReadAsStringAsync();
                Console.WriteLine(error);
                return new APIRespons { Status = false, StatusMessage = "فشل تعديل الخصم" };
            }
        }

        public async Task<APIRespons> RemoveDeductionAsync(string id, CheckAccess pass)
        {
            var check = await accountService.CheckAccess(pass);
            if (!check.Status)
            {
                return check;
            }

            var client = httpClientFactory1.CreateClient("API");
            var respons = await client.DeleteAsync($"api/Deduction/removeDeduction/{id}");

            if (respons.IsSuccessStatusCode)
            {
                return new APIRespons { Status = true, StatusMessage = "تم حذف الخصم بنجاح" };
            }
            else
            {
                return new APIRespons { Status = false, StatusMessage = "فشل حذف الخصم" };
            }
        }

        public async Task<List<DeductionMV>> GetAllDeduction(Pagenation pagenation)
        {
            var client = httpClientFactory1.CreateClient("API");
            var respons = await client.GetAsync($"api/Deduction/getAllDeduction?PageNumber={pagenation.PageNumber}&PageSize={pagenation.PageSize}");

            if (respons.IsSuccessStatusCode)
            {
                var deductions = await respons.Content.ReadFromJsonAsync<List<DeductionMV>>();
                return deductions ?? new List<DeductionMV>();
            }
            else
            {
                return new List<DeductionMV>();
            }
        }

        public async Task<List<DeductionMV>> GetBarberDeductionAsync(string id, Pagenation pagenation)
        {
            var client = httpClientFactory1.CreateClient("API");
            var respons = await client.GetAsync($"api/Deduction/getBarberDeduction/{id}?PageNumber={pagenation.PageNumber}&PageSize={pagenation.PageSize}");

            if (respons.IsSuccessStatusCode)
            {
                var deductions = await respons.Content.ReadFromJsonAsync<List<DeductionMV>>();
                return deductions ?? new List<DeductionMV>();
            }
            else
            {
                return new List<DeductionMV>();
            }
        }

        public async Task<List<DeductionMV>> GetDateDeductionAsync(DateTime date)
        {

            var client = httpClientFactory1.CreateClient("API");
            var respons = await client.GetAsync($"api/Deduction/getDeductionByDate?dateOnly={date}");

            if (respons.IsSuccessStatusCode)
            {
                var deductions = await respons.Content.ReadFromJsonAsync<List<DeductionMV>>();
                return deductions ?? new List<DeductionMV>();
            }
            else
            {
                return new List<DeductionMV>();
            }
        }

    }
}
