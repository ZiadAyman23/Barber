using Core.ModelView;

namespace BarberFront.Services
{
    public class AccountService
    {
        private readonly IHttpClientFactory httpClientFactory;

        public AccountService(IHttpClientFactory httpClientFactory)
        {
            this.httpClientFactory = httpClientFactory;
        }

        public async Task<APIRespons> CheckAccess(CheckAccess Pass)
        {
            var client = httpClientFactory.CreateClient("API");
            var respons = await client.PostAsJsonAsync($"api/Account/checkAccess",Pass);
            if (!respons.IsSuccessStatusCode)
            {
                string massage =await respons.Content.ReadAsStringAsync();
                return new APIRespons { Status = false, StatusMessage =massage  };
            }

            return new APIRespons { Status = true, StatusMessage = "" };
        }

        public async Task<Tokens> Login(Login login)
        {
            var client = httpClientFactory.CreateClient("API");
            var respons = await client.PostAsJsonAsync($"api/Account/login", login);
            if (!respons.IsSuccessStatusCode)
            {
                return new Tokens();
            }
            var tokens = await respons.Content.ReadFromJsonAsync<Tokens>();
            return tokens ?? new Tokens() ;
        }
    }
}
