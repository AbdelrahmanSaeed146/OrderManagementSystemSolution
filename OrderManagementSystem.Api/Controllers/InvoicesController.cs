using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OrderManagementSystem.Core;
using OrderManagementSystem.Core.Entities;
using OrderManagementSystem.Core.Services;


namespace OrderManagementSystem.Api.Controllers
{

    public class InvoicesController : BaseApiController
    {

        private readonly IUnitOfWork _unitOfWork;
        private readonly IInvoiceService _invoiceService;

        public InvoicesController(IUnitOfWork unitOfWork , IInvoiceService invoiceService)
        {
            _unitOfWork = unitOfWork;
            _invoiceService = invoiceService;
        }

        [HttpGet("{invoiceId}")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<Invoice>> GetInvoiceById(int invoiceId)
        {
            var invoice = await _invoiceService.GetInvoiceByIdAsync(invoiceId);
            if (invoice == null)
                return NotFound();

            return Ok(invoice);
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<IReadOnlyList<Invoice>>> GetAllInvoices()
        {
            var invoices = await _invoiceService.GetAllInvoicesAsync();
            if (invoices is null)
                return NotFound();
            return Ok(invoices);
        }
    }
}
