using Stripe;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderManagementSystem.Core.Entities.Dtos
{
    public class OrderDto
    {
        //public int Id { get; set; }
        //public DateTime OrderDate { get; set; }
        //public decimal TotalAmount { get; set; }
        //public List<OrderItemDto> OrderItems { get; set; }


        public string BuyerEmail { get; set; }

        [Required]
        public string PaymentMethod { get; set; }

        [Required]
        public List<OrderItem> Items { get; set; }

        public PaymentIntent PaymentIntent { get; set; } 

    }
}
