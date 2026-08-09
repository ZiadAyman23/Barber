namespace Infrastructer.Service
{
    public class ItemService : IItemService
    {
        private readonly IItemRepo _itemRepo;

        public ItemService(IItemRepo itemRepo)
        {
            _itemRepo = itemRepo;
        }

        public async Task AddItem(ItemD itemD)
        {
            Item item = new Item
            {
                Name = itemD.Name,
                Price = itemD.Price
            };
            await _itemRepo.AddItem(item);
        }

        public async Task<ItemMV> GetItemById(string id)
        {
            var item = await _itemRepo.GetItem(id);
            return new ItemMV
            {
                Id = item.Id,
                Name = item.Name,
                Price = item.Price
            };
        }

        public async Task<List<ItemMV>> GetItems()
        {
            var items = await _itemRepo.GetItems();
            return items.Select(item => new ItemMV
            {
                Id = item.Id,
                Name = item.Name,
                Price = item.Price
            }).ToList();
        }

        public async Task RemoveItem(string id)
        {
            await _itemRepo.RemoveItem(id);
        }

        public async Task UpdateItem(ItemD itemD, string id)
        {
            var item = await _itemRepo.GetItem(id);
            item.Name = itemD.Name;
            item.Price = itemD.Price;
            await _itemRepo.UpdateItem(item);
        }
    }
}
