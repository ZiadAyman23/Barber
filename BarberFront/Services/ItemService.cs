using Core.Models;
using Core.ModelView;

namespace BarberFront.Services
{
    public class ItemService
    {
        readonly IHttpClientFactory _httpClientFactory;
        readonly AccountService _accountService;

        public ItemService(IHttpClientFactory httpClientFactory, AccountService accountService)
        {
            _httpClientFactory = httpClientFactory;
            _accountService = accountService;
        }

        public async Task<List<ItemMV>> GetAllItems()
        {
            var client = _httpClientFactory.CreateClient("API");
            var response = await client.GetAsync("api/Item/getAllItems");
            if (response.IsSuccessStatusCode)
            {
                var items = await response.Content.ReadFromJsonAsync<List<ItemMV>>();
                return items ?? new List<ItemMV>();
            }
            return new List<ItemMV>();
        }

        public async Task<ItemOrder> GetItemById(string id)
        {
            var client = _httpClientFactory.CreateClient("API");
            var response = await client.GetAsync($"api/Item/getItemById/{id}");
            if (response.IsSuccessStatusCode)
            {
                var item = await response.Content.ReadFromJsonAsync<ItemOrder>();
                return item ?? new ItemOrder();
            }
            return new ItemOrder();
        }

        public async Task<APIRespons> AddItem(ItemD item)
        {
            var client = _httpClientFactory.CreateClient("API");
            var response = await client.PostAsJsonAsync("api/Item/addItem", item);
            APIRespons aPIRespons = new APIRespons();
            if (!response.IsSuccessStatusCode)
            {
                aPIRespons.Status = false;
                aPIRespons.StatusMessage = "فشل إضافة العنصر";
                return aPIRespons;
            }

            aPIRespons.Status = true;
            aPIRespons.StatusMessage = "تم إضافة العنصر بنجاح";
            return aPIRespons;
        }

        public async Task<APIRespons> DeleteItem(string id, CheckAccess pass)
        {
            var check = await _accountService.CheckAccess(pass);
            if (!check.Status)
            {
                return check;
            }

            var client = _httpClientFactory.CreateClient("API");
            var response = await client.DeleteAsync($"api/Item/deleteItem/{id}");
            APIRespons aPIRespons = new APIRespons();
            if (!response.IsSuccessStatusCode)
            {
                aPIRespons.Status = false;
                aPIRespons.StatusMessage = "فشل حذف العنصر";
                return aPIRespons;
            }

            aPIRespons.Status = true;
            aPIRespons.StatusMessage = "تم حذف العنصر بنجاح";
            return aPIRespons;
        }

        public async Task<APIRespons> UpdateItem(ItemD item, string id)
        {
            var client = _httpClientFactory.CreateClient("API");
            var response = await client.PutAsJsonAsync($"api/Item/updateItem/{id}", item);
            APIRespons aPIRespons = new APIRespons();
            if (!response.IsSuccessStatusCode)
            {
                aPIRespons.Status = false;
                aPIRespons.StatusMessage = "فشل تعديل البيانات";
                return aPIRespons;
            }

            aPIRespons.Status = true;
            aPIRespons.StatusMessage = "تم تعديل البيانات بنجاح";
            return aPIRespons;
        }
    }
}
