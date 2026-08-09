namespace Core.IService
{
    public interface IOrderService
    {
        public Task<OrderMV> GetOrderAsync(string orderId);
        public Task<List<OrderMV>> GetAllOrdersAsync(Pagenation pagenation);
        public Task<List<OrderMV>> GetAllBarberOrdersAsync(Pagenation pagenation, string BarberID);
        public Task<List<OrderMV>> GetOrderByDate(DateTime dateTime);
        public Task AddOrderAsync(OrderD order);
        public Task DeleteOrderAsync(string orderId);
        public Task UpdateOrderAsync(OrderD order, string id);

    }
}
