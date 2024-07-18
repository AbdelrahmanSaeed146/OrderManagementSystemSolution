﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderManagementSystem.Core.Entities
{
    public class Customer :BaseEntity
    {
    
        public string Name { get; set; }

        [EmailAddress]
        public string Email { get; set; }

        public ICollection<Order> Orders { get; set; }
    }
}
