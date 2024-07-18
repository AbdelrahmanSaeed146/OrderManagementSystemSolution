using Microsoft.Extensions.Configuration;
using OrderManagementSystem.Core;
using OrderManagementSystem.Core.Entities;
using OrderManagementSystem.Core.Services;
using Stripe;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderManagementSystem.Services
{
    public class PaymentService : IPaymentService
    {

        private readonly IConfiguration _configuration;
        private readonly IUnitOfWork _unitOfWork;

        public PaymentService(IConfiguration configuration, IUnitOfWork unitOfWork)
        {
            _configuration = configuration;
            _unitOfWork = unitOfWork;
        }
        public async Task<PaymentIntent?> CreateOrUpdatePaymentIntent(string paymentIntentId, decimal amount)
        {
            StripeConfiguration.ApiKey = _configuration["StripeSettings:SecretKey"];

            var paymentIntentService = new PaymentIntentService();
            PaymentIntent paymentIntent;

            if (string.IsNullOrEmpty(paymentIntentId))
            {
                var options = new PaymentIntentCreateOptions
                {
                    Amount = (long)(amount * 100), 
                    Currency = "usd",
                    PaymentMethodTypes = new List<string> { "card" }
                };

                paymentIntent = await paymentIntentService.CreateAsync(options);
            }
            else
            {
                var options = new PaymentIntentUpdateOptions
                {
                    Amount = (long)(amount * 100) 
                };

                paymentIntent = await paymentIntentService.UpdateAsync(paymentIntentId, options);
            }

            return paymentIntent;
        }

        public async Task<Order?> UpdateOrderStatus(string paymentIntentId, bool isPaid)
        {
            var orderRepo = _unitOfWork.Repository<Order>();
            var order = await orderRepo.FirstOrDefaultAsync(o => o.PaymentIntentId == paymentIntentId);

            if (order == null) return null;

            order.Status = isPaid ? OrderStatus.PaymentReceived : OrderStatus.PaymentFaild;

            orderRepo.Update(order);
            await _unitOfWork.CompleteAsync();

            return order;
        }
    }
}
