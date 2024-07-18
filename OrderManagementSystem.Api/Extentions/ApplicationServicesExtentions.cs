using Microsoft.AspNetCore.Mvc;
using OrderManagementSystem.Core;
using OrderManagementSystem.Core.Services;
using OrderManagementSystem.Repsitory;
using OrderManagementSystem.Services;
using OrderManagementSystem.Services.AuthService;

namespace OrderManagementSystem.Api.Extentions
{
    public static class ApplicationServicesExtentions
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {
            services.AddScoped(typeof(IUnitOfWork), typeof(UnitOfWork));
            services.AddScoped(typeof(IPaymentService), typeof(PaymentService));
            services.AddScoped(typeof(IOrderService), typeof(OrderService));
            services.AddScoped(typeof(IProductService), typeof(ProductService));
            services.AddScoped(typeof(IInvoiceService), typeof(InvoiceService));
            services.AddScoped(typeof(IAuthService), typeof(AuthService));

            return services;
        }
    }
}
