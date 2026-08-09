using Core.ModelView;

namespace BarberFront.Services
{
    public class ExpenseService
    {
        private readonly IHttpClientFactory httpClientFactory;
        private readonly AccountService accountService;

        public ExpenseService(IHttpClientFactory httpClientFactory,AccountService accountService)
        {
            this.httpClientFactory = httpClientFactory;
            this.accountService = accountService;
        }


        public async Task<APIRespons> AddExpense(ExpenseD expense)
        {
            var client = httpClientFactory.CreateClient("API");
            var response = await client.PostAsJsonAsync("api/expense/add-expense", expense);

            APIRespons aPIRespons = new APIRespons();

            if (!response.IsSuccessStatusCode)
            {
                aPIRespons.Status = false;
                aPIRespons.StatusMessage = "فشل إضافة المصروف";
                return aPIRespons;
            }

            aPIRespons.Status = true;
            aPIRespons.StatusMessage = "تم إضافة المصروف بنجاح";
            return aPIRespons;


        }

        public async Task<APIRespons> UpdateExpense(string id, ExpenseD expense)
        {
            var client = httpClientFactory.CreateClient("API");
            var response = await client.PutAsJsonAsync($"api/expense/update-expense/{id}", expense);
            APIRespons aPIRespons = new APIRespons();
            if (!response.IsSuccessStatusCode)
            {
                aPIRespons.Status = false;
                aPIRespons.StatusMessage = "فشل تعديل المصروف";
                return aPIRespons;
            }

            aPIRespons.Status = true;
            aPIRespons.StatusMessage = "تم تعديل المصروف بنجاح";
            return aPIRespons;

        }

        public async Task<ExpenseMV> GetExpense(string id)
        {
            var client = httpClientFactory.CreateClient("API");
            var response = await client.GetAsync($"api/expense/get-expense/{id}");


            var expense = await response.Content.ReadFromJsonAsync<ExpenseMV>();
            return expense ?? throw new Exception("Expense not found");

        }

        public async Task<IEnumerable<ExpenseMV>> GetAllExpenses(Pagenation pagenation)
        {
            var client = httpClientFactory.CreateClient("API");
            var response = await client.GetAsync($"api/expense/get-all-expenses?PageNumber={pagenation.PageNumber}&PageSize={pagenation.PageSize}");

            var expenses = await response.Content.ReadFromJsonAsync<IEnumerable<ExpenseMV>>();
            return expenses ?? new List<ExpenseMV>();
        }

        public async Task<APIRespons> RemoveExpense(string id ,CheckAccess pass)
        {
            var check = await accountService.CheckAccess(pass);

            if (!check.Status)
            {
                return check;
            }

            var client = httpClientFactory.CreateClient("API");
            var response = await client.DeleteAsync($"api/expense/remove-expense/{id}");
            APIRespons aPIRespons = new APIRespons();
            if (response.IsSuccessStatusCode)
            {
                aPIRespons.Status = true;
                aPIRespons.StatusMessage = "تم حذف المصروف بنجاح";
                return aPIRespons;
            }
            else
            {
                aPIRespons.Status = false;
                aPIRespons.StatusMessage = "فشل حذف المصروف";
                return aPIRespons;
            }
        }


        public async Task<IEnumerable<ExpenseMV>> GetAllExpensesByDate(DateOnly date)
        {
            var client = httpClientFactory.CreateClient("API");
            var response = await client.GetAsync($"api/expense/getByDate?date={date}");


            var expenses = await response.Content.ReadFromJsonAsync<IEnumerable<ExpenseMV>>();
            return expenses ?? new List<ExpenseMV>();
        }


    }
}
