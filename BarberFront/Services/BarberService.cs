global using Core.DTO;
using Core.ModelView;
using Microsoft.JSInterop;
using System.Net.Http.Headers;

namespace BarberFront.Services
{
    public class BarberService
    {
        private readonly IHttpClientFactory httpClientFactory;
        private readonly AccountService accountService;
        private readonly IJSRuntime jSRuntime1;
        public BarberService(IHttpClientFactory httpClientFactory ,AccountService accountService, IJSRuntime jSRuntime)
        {
            this.httpClientFactory = httpClientFactory;
            this.accountService = accountService;
            this.jSRuntime1=jSRuntime;
        }


        private async Task<HttpClient> GetClient()
        {
            var client = httpClientFactory.CreateClient("API");

            var token = await jSRuntime1.InvokeAsync<string>("storage.getAccessToken");

            if (!string.IsNullOrWhiteSpace(token))
            {
                client.DefaultRequestHeaders.Authorization =
                    new AuthenticationHeaderValue("Bearer", token);
            }

            return client;
        }

        public async Task AddBarber(BarberD barber)
        {
            var client = await GetClient();

            var response = await client.PostAsJsonAsync("api/Barber/addBarber", barber);
            if (!response.IsSuccessStatusCode)
            {
                throw new Exception($"Failed to add barber. Status code: {response.StatusCode}");
            }

        }


        public async Task<APIRespons> EditBarber(string id, BarberD barber,CheckAccess checkAccess)
        {

            var res = await accountService.CheckAccess(checkAccess);
            if (!res.Status)
            {
                return res;
            }
            var client = await GetClient();

            var response = await client.PutAsJsonAsync($"api/Barber/updateBarber/{id}", barber);
            if (!response.IsSuccessStatusCode)
            {
                return new APIRespons { Status=false , StatusMessage= "فشل تعديل البيانات"};
            }
            return new APIRespons { Status = true, StatusMessage = "تم التعديل بنجاح" };
        }


        public async Task<APIRespons> RemoveBarber(string id,CheckAccess checkAccess)
        {

            var res = await accountService.CheckAccess(checkAccess);
            if (!res.Status)
            {
                return res;
            }
            var client = await GetClient();

            var response = await client.DeleteAsync($"api/Barber/deleteBarber/{id}");
            if (!response.IsSuccessStatusCode)
            {
                return new APIRespons { StatusMessage ="فشل الحذف" ,Status =false};
            }
            return new APIRespons { Status = true, StatusMessage = "تم الحذف بنجاح"};
        }



        public async Task<List<BarberMV>> GetAllBarbers()
        {
            var client = await GetClient();

            var response = await client.GetAsync("api/Barber/getAllBarbers");
            if (response.IsSuccessStatusCode)
            {
                var barbers = await response.Content.ReadFromJsonAsync<List<BarberMV>>();
                return barbers ?? new List<BarberMV>();
            }
            else
            {
                throw new Exception($"Failed to retrieve barbers. Status code: {response.StatusCode}");
            }
        }




        public async Task<BarberMV> GetBarberById(string id)
        {
            var client = await GetClient();

            var response = await client.GetAsync($"api/Barber/getBarberById/{id}");
            if (response.IsSuccessStatusCode)
            {
                var barber = await response.Content.ReadFromJsonAsync<BarberMV>();
                return barber ?? throw new Exception("Barber not found");
            }
            else
            {
                throw new Exception($"Failed to retrieve barber. Status code: {response.StatusCode}");
            }
        }
    }
}
