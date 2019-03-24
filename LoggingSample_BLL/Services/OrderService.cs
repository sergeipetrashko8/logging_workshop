using LoggingSample_BLL.Helpers;
using LoggingSample_BLL.Models;
using LoggingSample_DAL.Context;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;

namespace LoggingSample_BLL.Services
{
    public class OrderService : IDisposable
    {
        private readonly AppDbContext _context = new AppDbContext();

        public Task<OrderModel> GetOrder(int customerId, int orderId)
        {
            return _context.Orders
                .SingleOrDefaultAsync(item => item.Id == orderId && item.CustomerId == customerId)
                .ContinueWith(task =>
                {
                    var order = task.Result;

                    return order?.Map();
                });
        }

        public Task<IEnumerable<OrderModel>> GetAllOrders(int customerId)
        {
            return _context.Orders.Where(order => order.CustomerId == customerId).ToArrayAsync().ContinueWith(task =>
            {
                var orders = task.Result.Select(order => order.Map());

                return orders;
            });
        }

        public void Dispose()
        {
            _context?.Dispose();
        }
    }
}