using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using OrderManagementSystem.Core;
using OrderManagementSystem.Core.Entities;
using OrderManagementSystem.Core.Entities.Dtos;
using OrderManagementSystem.Core.Services;
using Stripe;

namespace OrderManagementSystem.Api.Controllers
{

    public class OrdersController : BaseApiController
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IOrderService _orderService;
        private readonly IEmailService _emailService;

        public OrdersController(IUnitOfWork unitOfWork , IOrderService orderService , IEmailService emailService)
        {
            _unitOfWork = unitOfWork;
            _orderService = orderService;
            _emailService = emailService;
        }


        [HttpPost] // ok
        public async Task<ActionResult<IReadOnlyList<Order>>> CreateOrder( OrderDto createOrderDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

           
                var order = await _orderService.CreateOrderAsync(createOrderDto.BuyerEmail,
                                                                    createOrderDto.PaymentMethod,
                                                                    createOrderDto.Items,
                                                                    createOrderDto.PaymentIntent);
                if (order == null)
                {
                    return BadRequest("Failed");
                }
            
            return Ok (order);
            
         
        }


        [ProducesResponseType(typeof(Order), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(BadRequest), StatusCodes.Status400BadRequest)]
        [HttpGet("{orderId}")] // ok
        public async Task<ActionResult<IReadOnlyList<Order>>> GetOrderById(int orderId)
        {
            var order = await _unitOfWork.Repository<Order>().GetByIdAsync(orderId);
            if (order == null)
                return NotFound();

            return Ok(order);
        }

        [HttpGet]
        [Authorize(Roles = "Admin")] // ok
        public async Task<ActionResult<IReadOnlyList<Order>>> GetAllOrders()
        {
            var orders = await _unitOfWork.Repository<Order>().GetAllAsync();
            return Ok(orders);
        }

        [HttpPut("{orderId}/status")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<Order>> UpdateOrderStatus(int orderId, OrderStatus status)
        {
            var order = await _unitOfWork.Repository<Order>().GetByIdAsync(orderId);
            if (order == null)
                return NotFound();

            order.Status = status;

            _unitOfWork.Repository<Order>().Update(order);
            var result = await _unitOfWork.CompleteAsync();
            //var customer = await _unitOfWork.Repository<OrderManagementSystem.Core.Entities.Customer>().FirstOrDefaultAsync(c => c.customer == order.CustomerId);
            //_emailService.SendEmailAsync(customer.Email, "order update", "your order updated");
            if (result >= 0) { return Ok(order); }
            else { return BadRequest(); }
          
        }

        private decimal CalculateDiscount(decimal totalAmount)
        {
            if (totalAmount > 200)
                return 0.10m;
            else if (totalAmount > 100)
                return 0.05m;
            return 0m;
        }
    }
}
