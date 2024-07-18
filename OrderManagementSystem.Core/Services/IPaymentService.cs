using OrderManagementSystem.Core.Entities;
using Stripe;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderManagementSystem.Core.Services
{
    public interface IPaymentService
    {

        Task<PaymentIntent?> CreateOrUpdatePaymentIntent(string paymentIntentId, decimal amount);
        Task<Order?> UpdateOrderStatus(string paymentIntentId, bool isPaid);
    }
}
