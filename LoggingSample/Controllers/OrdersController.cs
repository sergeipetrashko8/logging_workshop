using System;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Routing;
using LoggingSample_BLL.Models;
using LoggingSample_BLL.Services;
using NLog;

namespace LoggingSample.Controllers
{
    [RoutePrefix("api")]
    public class OrdersController : ApiController
    {
        private readonly OrderService _orderService = new OrderService();
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        [Route("customers/{customerId}/orders", Name = "Orders")]
        public async Task<IHttpActionResult> Get(int customerId)
        {
            Logger.Info("Start getting all orders.");

            var orders = await _orderService.GetAllOrders(customerId);

            Logger.Info("Retrieving orders to response.");
            
            return Ok(orders.Select(InitOrder));
        }

        [Route("customers/{customerId}/orders/{orderId}", Name = "Order")]
        public async Task<IHttpActionResult> Get(int customerId, int orderId)
        {
            Logger.Info($"Start getting order with id {orderId} and customerId {customerId}.");

            try
            {
                var order = await _orderService.GetOrder(customerId, orderId);

                if (order == null)
                {
                    Logger.Info($"No order with id {orderId} and customerId {customerId} was found.");
                    return NotFound();
                }

                Logger.Info($"Retrieving order with id {orderId} and customerId {customerId} to response.");

                return Ok(InitOrder(order));
            }
            catch (Exception ex)
            {
                Logger.Error(ex, $"Some error occured while getting orderId {orderId} and customerId {customerId}.");
                throw;
            }
        }

        private object InitOrder(OrderModel model)
        {
            return new
            {
                _self = new UrlHelper(Request).Link("Order", new {customerId = model.CustomerId, orderId = model.Id}),
                customer = new UrlHelper(Request).Link("Customer", new {customerId = model.CustomerId}),
                data = model
            };
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _orderService.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
 