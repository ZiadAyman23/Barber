using Infrastructer.Context;

namespace Infrastructer.Repository
{
    public class OrderRepo : IOrderRepo
    {
        private readonly DataContext dataContext;

        public OrderRepo(DataContext dataContext)
        {
            this.dataContext = dataContext;
        }

        public async Task<int> GetMonthelyBarberOrders(GenralDuration genral)
        {
            var count = await dataContext.Orders.Where(e=>e.BarberId==genral.BarberId && e.Date>= genral.StartDate && e.Date<genral.EndDate).CountAsync();
            return count;
        }

        public async Task<(decimal salary, int Count)> GetMonthelyReport(DateTime start, DateTime end)
        {
            var ordersSales = await dataContext.Orders.Where(e=>e.Date>= start && e.Date<end).SumAsync(e => e.TotalPrice);
            var OrderCount = await dataContext.Orders.Where(e => e.Date >= start && e.Date < end).CountAsync();
            return (ordersSales, OrderCount);
        }

        public async Task<List<Order>> GetOrderByDate(DateTime dateTime)
        {
            var start = dateTime.Date;
            var end = start.AddDays(1);
            var data = await dataContext.Orders
                .Include(e => e.Barber)
                .Where(e => e.Date >= start && e.Date < end)
                .OrderByDescending(e => e.Date)
                .AsNoTracking()
                .ToListAsync();

            return data;
        }

        public async Task<List<Order>> GetOrderByDateRange( GenralDuration duration)
        {
            var orders =await dataContext.Orders.Include(e => e.Barber)
                .Where(e => e.BarberId == duration.BarberId && e.Date >= duration.StartDate && e.Date <= duration.EndDate)
                .OrderByDescending(e => e.Date)
                .AsNoTracking()
                .ToListAsync();
            return orders;

        }

        public async Task<(decimal, int)> GetOrdersByBarberId(string barberId, DateTime startDate)
        {

            var start = startDate;
            var end = DateTime.Now;
            var totalSales = await dataContext.Orders.Where(e => e.BarberId == barberId && e.Date >= start && e.Date <= end).SumAsync(e => e.TotalPrice);
            var orderCount = await dataContext.Orders.Where(e => e.BarberId == barberId && e.Date >= start && e.Date <= end).CountAsync();

            return (totalSales, orderCount);
        }

        async Task IOrderRepo.AddOrder(Order order)
        {
            dataContext.Orders.Add(order);
            await dataContext.SaveChangesAsync();

        }

        async Task IOrderRepo.DeleteOrder(string id)
        {
            var order = await dataContext.Orders.FirstOrDefaultAsync(o => o.Id == id);
            if (order == null)
                throw new KeyNotFoundException(nameof(order));

            dataContext.Orders.Remove(order);
            await dataContext.SaveChangesAsync();
        }

        async Task<List<Order>> IOrderRepo.GetAllBarberOrders(string BarberID, Pagenation pagenation)
        {
            var data = await dataContext.Orders
                .Where(e => e.BarberId == BarberID)
                .OrderByDescending(e => e.Date)
                .Skip((pagenation.PageNumber - 1) * pagenation.PageSize)
                .Take(pagenation.PageSize)
                .AsNoTracking()
                .ToListAsync();
            return data;
        }

        async Task<List<Order>> IOrderRepo.GetAllOrders(Pagenation pagenation)
        {
            var data = await dataContext.Orders
                .Include(e => e.Barber)
                .OrderByDescending(e => e.Date)
                .Skip((pagenation.PageNumber - 1) * pagenation.PageSize)
                .Take(pagenation.PageSize)
                .AsNoTracking()
                .ToListAsync();
            return data;
        }

        async Task<Order> IOrderRepo.GetOrderById(string id)
        {
            var data =await dataContext.Orders.Include(e=>e.Items).Include(e=>e.Barber).AsNoTracking().FirstOrDefaultAsync(e => e.Id== id);
            if(data == null)
                throw new KeyNotFoundException(nameof(data));

            return data;
        }

        async Task IOrderRepo.UpdateOrder(Order order)
        {

           dataContext.Orders.Update(order);
            await dataContext.SaveChangesAsync();
        }
    }
}
