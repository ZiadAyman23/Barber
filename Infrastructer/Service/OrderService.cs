using System;
using System.Collections.Generic;
using System.Text;

namespace Infrastructer.Service
{
    public class OrderService : IOrderService
    {
        private readonly IOrderRepo orderRepo;

        public OrderService(IOrderRepo orderRepo)
        {
            this.orderRepo = orderRepo;
        }

        public async Task<List<OrderMV>> GetOrderByDate(DateTime dateTime)
        {
            var orders = await orderRepo.GetOrderByDate(dateTime);
            List<OrderMV> result = new List<OrderMV>();
            foreach (var item in orders)
            {
                result.Add(new OrderMV
                {
                    BarberName = item.Barber.Name,
                    id = item.Id,
                    TotalPrice = item.TotalPrice,
                    Date = item.Date,
                });
            }
            
            return result;
        }

        async Task IOrderService.AddOrderAsync(OrderD order)
        {
           Order order1 = new Order();
            order1.BarberId = order.BarberId;
            order1.TotalPrice = order.TotalPrice;
   
            foreach (var item in order.orderItemDs)
            {

                order1.Items.Add(new ItemOrder { Name=item.Name, Price=item.Price });

            }

            await orderRepo.AddOrder(order1);
        }

        async Task IOrderService.DeleteOrderAsync(string orderId)
        {
            await orderRepo.DeleteOrder(orderId);
        }

        async Task<List<OrderMV>> IOrderService.GetAllBarberOrdersAsync(Pagenation pagenation, string BarberID)
        {
            var orders =await orderRepo.GetAllBarberOrders(BarberID,pagenation);
            List<OrderMV> result = new List<OrderMV>();
            foreach(var item in orders)
            {
                result.Add(new OrderMV
                {
                    id = item.Id,
                    TotalPrice = item.TotalPrice,
                    Date=item.Date,
                });
            }
            return result;
            
        }

        async Task<List<OrderMV>> IOrderService.GetAllOrdersAsync(Pagenation pagenation)
        {
            var orders = await orderRepo.GetAllOrders(pagenation);
            List<OrderMV> result = new List<OrderMV>();
            foreach (var item in orders)
            {
                result.Add(new OrderMV
                {
                    BarberName = item.Barber.Name,
                    id = item.Id,
                    TotalPrice = item.TotalPrice,
                    Date = item.Date,
                });
            }
            return result;

        }

        async Task<OrderMV> IOrderService.GetOrderAsync(string orderId)
        {
            var order = await orderRepo.GetOrderById(orderId);
            List<ItemOrderMV> itemOrderMVs=new List<ItemOrderMV>();

            foreach (var item in order.Items)
            {
                itemOrderMVs.Add(new ItemOrderMV 
                {
                    price=item.Price,
                    name=item.Name,
                    id=item.Id,

                });         
            }

            OrderMV result = new OrderMV()
            {
                BarberName = order.Barber.Name,
                BarberId = order.BarberId,
                id = order.Id,
                TotalPrice = order.TotalPrice,
                Date = order.Date,
                Items = itemOrderMVs
            };

            return result;
          
        }

        async Task IOrderService.UpdateOrderAsync(OrderD order, string id)
        {
            var _order = await orderRepo.GetOrderById(id);

            List<ItemOrder> itemOrder = new List<ItemOrder>();
            _order.Items.Clear();

            foreach (var item in order.orderItemDs)
            {
                itemOrder.Add(new ItemOrder
                {
                    Price = item.Price,
                    Name = item.Name,
                });
            }


            _order.TotalPrice=order.TotalPrice;
            _order.BarberId = order.BarberId;
            _order.Items = itemOrder;


            
            await orderRepo.UpdateOrder(_order);
            
        }
    }
}
