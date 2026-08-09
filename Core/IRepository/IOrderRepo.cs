namespace Core.IRepository
{
    public interface IOrderRepo
    {
        public Task AddOrder(Order order);
        public Task DeleteOrder(string id);
        public Task UpdateOrder(Order order);
        public Task<Order> GetOrderById(string id);
        public Task<List<Order>> GetAllOrders(Pagenation pagenation);
        public Task<List<Order>> GetOrderByDate(DateTime dateTime);
        public Task<List<Order>> GetAllBarberOrders(string BarberID,Pagenation pagenation);
        public Task<(decimal, int)> GetOrdersByBarberId(string barberId, DateTime startDate);
        public Task<List<Order>> GetOrderByDateRange( GenralDuration duration);

    }
}
