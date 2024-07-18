using OrderManagementSystem.Core.Entities;
using Stripe;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderManagementSystem.Core.Services
{
    public interface IOrderService
    {

        Task<Order?> CreateOrderAsync(string buyerEmail, string PaymentMethod,  List<OrderItem> items , PaymentIntent paymentintent);

        Task<Order?> GetOrderByIdForUserAsync(string BuyerEmail, int OrderId);

        Task<IReadOnlyList<Order>> GetOrdersForUserAsync(string BuyerEmail);
    }
}
