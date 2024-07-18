using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using OrderManagementSystem.Core;
using OrderManagementSystem.Core.Entities;
using OrderManagementSystem.Core.Entities.Dtos;
using OrderManagementSystem.Core.Services;

namespace OrderManagementSystem.Api.Controllers
{
  
    public class CustmersController : BaseApiController
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly UserManager<User> _userManager;
        private readonly IAuthService _authService;

        public CustmersController(IUnitOfWork unitOfWork , UserManager<User> userManager , IAuthService authService )
        {
            _unitOfWork = unitOfWork;
            _userManager = userManager;
            _authService = authService;
        }

      

        [HttpPost]
        public async Task<ActionResult<CustomerDto>> CreateCustomer(CustomerDto customerDto)
        {
            var customer = new Customer()
            {
                Name = customerDto.name,
                Email = customerDto.Email,

            };

            _unitOfWork.Repository<Customer>().Add(customer);
            var user = new User()
            {
                DisplayName = customer.Name,
                Email = customer.Email,
                Role = "Customer",
                PasswordHash = customerDto.Password,
            };

            var result = await _userManager.CreateAsync(user, customerDto.Password);
            if (!result.Succeeded) { return BadRequest(result.Errors.Select(E => E.Description)); }
            await _userManager.AddToRoleAsync(user, "Customer");
            return Ok(new UserDto() { Name = user.DisplayName, Email = user.Email, Token = await _authService.GetTokenAsync(user, _userManager) });
        }

        [HttpGet("{customerId}/orders")]
        public async Task<ActionResult<IReadOnlyList<OrderDto>>> GetAllOrdersForCustomer(int customerId)
        {
            var customer = await _unitOfWork.Repository<Customer>().GetByIdAsync(customerId);
            if (customer is null)
            {
                return NotFound();
            }

            var orders = await _unitOfWork.Repository<Order>().GetAllWithFilters(o => o.Customer.Id == customerId);
            if (orders is null) return NotFound();
           

            return Ok(orders);
        }

        




    }
}
