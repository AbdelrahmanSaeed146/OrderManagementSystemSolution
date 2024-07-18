using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderManagementSystem.Core.Entities
{
    public class Order : BaseEntity
    {


        public Order()
        {
            
        }
        public Order(decimal totalAmount, string paymentMethod, ICollection<OrderItem> orderItems, string paymentIntentId , Customer customer)
        {
            Customer = customer;
            TotalAmount = totalAmount;
            PaymentMethod = paymentMethod;
            OrderItems = orderItems;
            PaymentIntentId = paymentIntentId;
        }


    
        public DateTime OrderDate { get; set; }
        public decimal TotalAmount { get; set; }
        public string PaymentMethod { get; set; }
        public OrderStatus Status { get; set; } = OrderStatus.Pending;
        public Customer Customer { get; set; }
        public ICollection<OrderItem> OrderItems { get; set; } = new HashSet<OrderItem>();
        public Invoice Invoice { get; set; }

        public string PaymentIntentId { get; set; }
    }
}
