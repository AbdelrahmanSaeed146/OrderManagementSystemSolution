﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderManagementSystem.Core.Services
{
    public interface IEmailService
    {
        public Task SendEmailAsync(string email , string subject , string body);
    }
}
