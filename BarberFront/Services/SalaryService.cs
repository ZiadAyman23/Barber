using Core.ModelView;

namespace BarberFront.Services
{
    public class SalaryService
    {
        private readonly IHttpClientFactory httpClientFactory;
        private readonly AccountService accountService;

        public SalaryService(IHttpClientFactory httpClientFactory,AccountService accountService)
        {
            this.httpClientFactory = httpClientFactory;
            this.accountService = accountService;
        }

        public async Task<List<CurrentSalary>> GetCurrentSalaries()
        {
            var client = httpClientFactory.CreateClient("API");
            var response = await client.GetAsync("api/Salary/GetCurrentSalaries");

            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadFromJsonAsync<List<CurrentSalary>>();
                return content ?? new List<CurrentSalary>();
            }
            else
            {
                // Handle error response
                throw new Exception($"Failed to retrieve current salaries. Status code: {response.StatusCode}");
            }
        }


        public async Task<APIRespons> AddSalary(SalaryD salary, CheckAccess checkAccess)
        {

            var res = await accountService.CheckAccess(checkAccess);
            if (!res.Status)
            {
                return res;
            }
            var client = httpClientFactory.CreateClient("API");
            var response = await client.PostAsJsonAsync("api/Salary/AddSalary", salary);
            if (!response.IsSuccessStatusCode)
            {
                // Handle error response
                return new APIRespons { Status = false, StatusMessage = $"Failed to add salary. Status code: {response.StatusCode}" };
            }
            return new APIRespons { Status = true, StatusMessage = "Salary added successfully." };
        }


        public async Task<List<SalaryMV>> GetSalaryByBarberId(string barberId, Pagenation pagenation)
        {
            var client = httpClientFactory.CreateClient("API");
            var response = await client.GetAsync($"api/Salary/getBarberSalary/{barberId}?pageNumber={pagenation.PageNumber}&pageSize={pagenation.PageSize}");

            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadFromJsonAsync<List<SalaryMV>>();
                return content ?? new List<SalaryMV>();
            }
            else
            {
                // Handle error response
                return new List<SalaryMV>();

            }
        }

        public async Task<List<SalaryMV>> GetAllSalary(Pagenation pagenation)
        {
            var client = httpClientFactory.CreateClient("API");
            var response = await client.GetAsync($"api/Salary/getAllPaidSalary?pageNumber={pagenation.PageNumber}&pageSize={pagenation.PageSize}");
            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadFromJsonAsync<List<SalaryMV>>();
                Console.WriteLine($"Number of salaries retrieved: {content.Count}");
                return content ?? new List<SalaryMV>();
            }
            else
            {
                // Handle error response
                return new List<SalaryMV>();
            }
        }


        public async Task<SalaryDetails> GetSalaryDetails(string salaryId)
        {
            var client = httpClientFactory.CreateClient("API");
            var response = await client.GetAsync($"api/Salary/getSalaryDetails/{salaryId}");

            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadFromJsonAsync<SalaryDetails>();
                return content ?? new SalaryDetails();
            }
            else
            {
                // Handle error response
                return new SalaryDetails();
            }
        }
    }
}
