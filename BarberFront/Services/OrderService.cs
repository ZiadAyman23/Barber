using Core.ModelView;
using System.ComponentModel.Design.Serialization;

namespace BarberFront.Services
{
    public class OrderService
    {
        private readonly IHttpClientFactory httpClientFactory;
        private readonly AccountService accountService;

        public OrderService(IHttpClientFactory httpClientFactory, AccountService accountService)
        {
            this.httpClientFactory = httpClientFactory;
            this.accountService = accountService;
        }


        public async Task<APIRespons> AddOrder(OrderD orderD)
        {
            var client = httpClientFactory.CreateClient("API");
            var respons = await client.PostAsJsonAsync("api/Order/creatOrder", orderD);
            
            if(respons.IsSuccessStatusCode)
            {
                return new APIRespons { Status = true, StatusMessage = "تم إنشاء الطلب بنجاح" };
            }
            else
            {
                return new APIRespons { Status = false, StatusMessage = "فشل إنشاء الطلب" };

            }
        }

        public async Task<APIRespons> EditOrder(OrderD orderD, string id, CheckAccess pass)
        {
            var check = await accountService.CheckAccess(pass);
            if (!check.Status)
            {
                return check;
            }

            var client = httpClientFactory.CreateClient("API");
            var respons = await client.PutAsJsonAsync($"api/Order/editOrder/{id}", orderD);
            if (respons.IsSuccessStatusCode)
            {
                return new APIRespons { Status = true, StatusMessage = "تم تعديل الطلب بنجاح" };
            }
            else if(respons.StatusCode == System.Net.HttpStatusCode.NotFound)
            {
                return new APIRespons { Status = false, StatusMessage = "الطلب غير موجود" };

            }
            else
            {
                return new APIRespons { Status = false, StatusMessage = "فشل تعديل الطلب" };

            }
        }


        public async Task<APIRespons> RemoveOrder(string id, CheckAccess pass)
        {
            var check = await accountService.CheckAccess(pass);
            if (!check.Status)
            {
                return check;
            }

            var client = httpClientFactory.CreateClient("API");
            var respons = await client.DeleteAsync($"api/Order/removeOrder/{id}");
            if (respons.IsSuccessStatusCode)
            {
                return new APIRespons { Status = true, StatusMessage = "تم حذف الطلب بنجاح" };
            }
            else if (respons.StatusCode == System.Net.HttpStatusCode.NotFound)
            {
                return new APIRespons { Status = false, StatusMessage = "الطلب غير موجود" };

            }
            else
            {
                return new APIRespons { Status = false, StatusMessage = "فشل حذف الطلب" };

            }
        }


        public async Task<OrderMV> GetOrderById(string id)
        {
            var client = httpClientFactory.CreateClient("API");
            var respons = await client.GetAsync($"api/Order/getOrderById/{id}");

            if (respons.IsSuccessStatusCode)
            {
                var order = await respons.Content.ReadFromJsonAsync<OrderMV>();
                return order ?? new OrderMV();
            }

            return new OrderMV();
        }


        public async Task<List<OrderMV>> GetAllOrder(Pagenation pagenation)
        {
            var client = httpClientFactory.CreateClient("API");
            var respons = await client.GetAsync($"api/Order/getAllOrders?PageNumber={pagenation.PageNumber}&PageSize={pagenation.PageSize}");
            if (respons.IsSuccessStatusCode)
            {
                var data = await respons.Content.ReadFromJsonAsync<List<OrderMV>>();
                return data ?? new List<OrderMV>();
            }
            return new List<OrderMV>();
        }

        public async Task<List<OrderMV>> GetBarberOrders(Pagenation pagenation, string id)
        {
            var client = httpClientFactory.CreateClient("API");
            var respons = await client.GetAsync($"api/Order/getBarberOrders/{id}?PageNumber={pagenation.PageNumber}&PageSize={pagenation.PageSize}");
            if (respons.IsSuccessStatusCode)
            {
                var data = await respons.Content.ReadFromJsonAsync<List<OrderMV>>();
                return data ?? new List<OrderMV>();
            }
            return new List<OrderMV>();
        }

        public async Task<List<OrderMV>> GetOrdersByDate(DateTime date )
        {
            var client = httpClientFactory.CreateClient("API");
            var respons = await client.GetAsync($"api/Order/getOrderByDate?date={date}");
            if (respons.IsSuccessStatusCode)
            {
                var data = respons.Content.ReadFromJsonAsync<List<OrderMV>>().Result;
               
                return data ?? new List<OrderMV>();
            }
            else
            { return new List<OrderMV>(); }
        }
    }
}
