global using Core.IRepository;
using Infrastructer.Context;

namespace Infrastructer.Repository
{
    public class ItemRepo : IItemRepo
    {
        private readonly DataContext dataContext;

        public ItemRepo(DataContext dataContext)
        {
            this.dataContext = dataContext;
        }

        public async Task AddItem(Item item)
        {
            dataContext.Items.Add(item);
            await dataContext.SaveChangesAsync();
        }

        public async Task<Item> GetItem(string id)
        {
            var item = await dataContext.Items.FindAsync(id);
            if (item == null)
                throw new Exception("Item from Database not found");
            return item;
        }

        public async Task<List<Item>> GetItems()
        {
            return await dataContext.Items.AsNoTracking().ToListAsync();
        }

        public async Task RemoveItem(string id)
        {
            var item = await dataContext.Items.FindAsync(id);
            if (item == null)
                throw new Exception("Item from Database not found");

            dataContext.Items.Remove(item);
            await dataContext.SaveChangesAsync();
        }

        public async Task UpdateItem(Item item)
        {
            var existingItem = await dataContext.Items.FindAsync(item.Id);
            if (existingItem == null)
                throw new Exception("Item from Database not found");

            dataContext.Entry(existingItem).CurrentValues.SetValues(item);
            await dataContext.SaveChangesAsync();
        }
    }
}
