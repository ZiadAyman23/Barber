namespace Core.IRepository
{
    public interface IItemRepo
    {
        public Task AddItem(Item item);
        public Task RemoveItem(string id);
        public Task UpdateItem(Item item);
        public Task<Item> GetItem(string id);
        public Task<List<Item>> GetItems();
    }
}
