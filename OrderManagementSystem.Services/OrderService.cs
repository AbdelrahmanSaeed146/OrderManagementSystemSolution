using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using OrderManagementSystem.Core;
using OrderManagementSystem.Core.Entities;
using OrderManagementSystem.Core.Services;
using Stripe;
using Stripe.Climate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderManagementSystem.Services
{
    public class OrderService : IOrderService
    {

        private readonly IUnitOfWork _unitOfWork;
        private readonly IPaymentService _paymentService;
        private readonly IInvoiceService _invoiceService;

        public OrderService
            (
                IUnitOfWork unitOfWork,
                IPaymentService paymentService ,
                IInvoiceService invoiceService

            )
        {
            _unitOfWork = unitOfWork;
            _paymentService = paymentService;
            _invoiceService = invoiceService;
        }
        public async Task<OrderManagementSystem.Core.Entities.Order?> CreateOrderAsync(string buyerEmail, string PaymentMethod, List<OrderItem> items, PaymentIntent paymentintent)
        {

            var Customer = await _unitOfWork.Repository<OrderManagementSystem.Core.Entities.Customer>().FirstOrDefaultAsync(C => C.Email == buyerEmail);
               var actualOrderItems =new List<OrderItem>();
            foreach (var item in items)
            {
                if (item.Product.Stock >= item.Quantity)
                {
                    actualOrderItems.Add(item);
                }
            }

            var subtotal = actualOrderItems.Sum(item => item.UnitPrice * item.Quantity);
            if (subtotal > 100)
            {

                subtotal = actualOrderItems.Sum(item => item.UnitPrice * 0.95m * item.Quantity);
            }
            else if (subtotal > 200)
            {
                subtotal = actualOrderItems.Sum(item => item.UnitPrice * 0.9m * item.Quantity);
            }
            var paymentIntent = await _paymentService.CreateOrUpdatePaymentIntent(paymentintent.Id, subtotal );
            if (paymentIntent is null) return null;

            var order = new OrderManagementSystem.Core.Entities.Order
            (
                subtotal,
                PaymentMethod,
                actualOrderItems,
                paymentIntent.Id,
                Customer
            );

            _unitOfWork.Repository<OrderManagementSystem.Core.Entities.Order>().Add(order);

            var result = await _unitOfWork.CompleteAsync();
            if (result <= 0) return null;
     
            return order;
        }

        public async Task<OrderManagementSystem.Core.Entities.Order?> GetOrderByIdForUserAsync(string BuyerEmail, int OrderId)
        {
            
            var Customer = await _unitOfWork.Repository<OrderManagementSystem.Core.Entities.Customer>().FirstOrDefaultAsync(C => C.Email == BuyerEmail);
            var order = await _unitOfWork.Repository<OrderManagementSystem.Core.Entities.Order>().FirstOrDefaultAsync(O => O.Id == OrderId && O.Customer.Id == Customer.Id);
            if (order is null) return null;
            return order;


         
        }

        public async Task<IReadOnlyList<OrderManagementSystem.Core.Entities.Order>> GetOrdersForUserAsync(string BuyerEmail)
        {
            var Customer = await _unitOfWork.Repository<OrderManagementSystem.Core.Entities.Customer>().FirstOrDefaultAsync(C => C.Email == BuyerEmail);
            var orders = await _unitOfWork.Repository<OrderManagementSystem.Core.Entities.Order>().GetAllWithFilters(O => O.Customer.Id == Customer.Id);
            if (orders is null) return null;
            return orders;
        }

      
    }
}
