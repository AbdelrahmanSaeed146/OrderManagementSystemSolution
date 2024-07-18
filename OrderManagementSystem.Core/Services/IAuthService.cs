using Microsoft.AspNetCore.Identity;
using OrderManagementSystem.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderManagementSystem.Core.Services
{
    public interface IAuthService
    {
        Task<string> GetTokenAsync(User user, UserManager<User> userManager);
    }
}
