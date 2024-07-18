using OrderManagementSystem.Core;
using OrderManagementSystem.Core.Entities;
using OrderManagementSystem.Core.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderManagementSystem.Services
{
    public class InvoiceService : IInvoiceService
    {
        private readonly IUnitOfWork _unitOfWork;

        public InvoiceService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<Invoice?> GetInvoiceByIdAsync(int invoiceId)
        {
            var invoice = await _unitOfWork.Repository<Invoice>().GetByIdAsync(invoiceId);
            return invoice;
        }

        public async Task<IReadOnlyList<Invoice>> GetAllInvoicesAsync()
        {
            var invoices = await _unitOfWork.Repository<Invoice>().GetAllAsync();
            return invoices.ToList();
        }

  
    }
}
