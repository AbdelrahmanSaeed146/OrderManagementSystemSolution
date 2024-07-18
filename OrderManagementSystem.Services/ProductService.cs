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
    public class ProductService : IProductService
    {
        private readonly IUnitOfWork _unitOfWork;

        public ProductService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<IReadOnlyList<Product>> GetAllProductsAsync()
        {
            return await _unitOfWork.Repository<Product>().GetAllAsync();
        }

        public async Task<Product?> GetProductByIdAsync(int productId)
        {
            return await _unitOfWork.Repository<Product>().GetByIdAsync(productId);
        }

        public async Task<Product> AddProductAsync(Product product)
        {
            _unitOfWork.Repository<Product>().Add(product);
            await _unitOfWork.CompleteAsync();
            return product;
        }

        public async Task<Product?> UpdateProductAsync( Product updatedProduct)
        {
            var productToUpdate = await _unitOfWork.Repository<Product>().GetByIdAsync(updatedProduct.Id);
            if (productToUpdate is null)
            {
                return null;
            }

            productToUpdate.Name = updatedProduct.Name;
            productToUpdate.Price = updatedProduct.Price;
            productToUpdate.Stock = updatedProduct.Stock;

            _unitOfWork.Repository<Product>().Update(productToUpdate);
            await _unitOfWork.CompleteAsync();

            return productToUpdate;
        }
    }
}
