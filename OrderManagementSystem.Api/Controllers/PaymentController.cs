using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OrderManagementSystem.Core;
using OrderManagementSystem.Core.Entities;
using OrderManagementSystem.Core.Services;
using Stripe;
using Stripe.Climate;

namespace OrderManagementSystem.Api.Controllers
{
   
    public class PaymentController : BaseApiController
    {
        private readonly IPaymentService _paymentService;
        private readonly IUnitOfWork _unitOfWork;

        private const string endpointSecret = "whsec_fe565e1c7e2ad966fe8a2f5183e46d69b108d0c88292b88cc68d224c5d4489b1";

        public PaymentController(IPaymentService paymentService , IUnitOfWork unitOfWork)
        {
            _paymentService = paymentService;
            _unitOfWork = unitOfWork;
        }


        [ProducesResponseType(typeof(OrderManagementSystem.Core.Entities.Order), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [Authorize]
        [HttpPost("{orderId}")]
        public async Task<IActionResult> CreateOrUpdatePaymentIntent( string orderId)
        {
            var order = await _unitOfWork.Repository<OrderManagementSystem.Core.Entities.Order>().GetByIdAsync(int.Parse(orderId));

            var PaymentIntent = await _paymentService.CreateOrUpdatePaymentIntent(order.PaymentIntentId, order.TotalAmount);
            if (PaymentIntent is null) return BadRequest();
            order.PaymentIntentId = PaymentIntent.Id;
            return Ok(order);
        }


        [HttpPost("webhook")]

        public async Task<IActionResult> WebHook()
        {
            var json = await new StreamReader(HttpContext.Request.Body).ReadToEndAsync();

            var stripeEvent = EventUtility.ConstructEvent(json,
                Request.Headers["Stripe-Signature"], endpointSecret);

            var paymenyIntent = (PaymentIntent)stripeEvent.Data.Object;

            if (stripeEvent.Type == Events.PaymentIntentSucceeded)
            {
                await _paymentService.UpdateOrderStatus(paymenyIntent.Id, true);
            }
            else if (stripeEvent.Type == Events.PaymentIntentPaymentFailed)
            {
                await _paymentService.UpdateOrderStatus(paymenyIntent.Id, false);
            }
            return Ok();
        }
    }
}
