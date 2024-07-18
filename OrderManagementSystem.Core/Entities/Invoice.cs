using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OrderManagementSystem.Core.Entities;

namespace OrderManagementSystem.Core.Entities
{
    public class Invoice :BaseEntity
    {
       
        public DateTimeOffset InvoiceDate { get; set; } = DateTimeOffset.UtcNow;
        public decimal TotalAmount { get; set; }
        public int OrderId { get; set; }
        public Order Order { get; set; }
    }
}

