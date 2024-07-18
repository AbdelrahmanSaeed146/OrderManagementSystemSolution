using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata;
using OrderManagementSystem.Core;
using OrderManagementSystem.Core.Entities;
using OrderManagementSystem.Core.Services;

namespace OrderManagementSystem.Api.Controllers
{

    public class ProductsController : BaseApiController
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IProductService _productService;

        public ProductsController(IUnitOfWork unitOfWork , IProductService productService)
        {
            _unitOfWork = unitOfWork;
            _productService = productService;
        }


        [HttpGet]
        public async Task<ActionResult<IReadOnlyList<Product>>> GetAllProducts()
        {
            var products = await _productService.GetAllProductsAsync();
            return Ok(products);
       }   

          [HttpGet("{productId}")]
        public async Task<IActionResult> GetProductById(int productId)
        {
            var product = await _productService.GetProductByIdAsync(productId);
            if (product == null)
                return NotFound();

            return Ok(product);
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<Product>> AddProduct(Product product)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            await _productService.AddProductAsync(product);
            await _unitOfWork.CompleteAsync();

            return Ok(product);
        }

        [HttpPut("{productId}")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<Product>> UpdateProduct( int productId ,Product updatedProduct)
        {

            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            var product = await _productService.GetProductByIdAsync(productId);
            if (product == null) { return NotFound(); }
            product = updatedProduct;
            var FinalProduct = await _productService.UpdateProductAsync(product);
            if (FinalProduct is null)
                return NotFound();

            await _unitOfWork.CompleteAsync();

            return Ok(FinalProduct);
        }
    }
}
