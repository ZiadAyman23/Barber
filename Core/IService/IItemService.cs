namespace Core.IService
{
    public interface IItemService
    {
        public Task AddItem(ItemD itemD);
        public Task RemoveItem(string id);
        public Task UpdateItem(ItemD itemD, string id);
        public Task<ItemMV> GetItemById(string id);
        public Task<List<ItemMV>> GetItems();
    }
}
