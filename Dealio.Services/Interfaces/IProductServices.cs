using Dealio.Domain.Entities;
using Dealio.Services.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dealio.Services.Interfaces
{
    public interface IProductServices
    {
        public Task<ServiceResult<Product>> CreateProduct(Product product);
        public Task<ServiceResult<Product>> UpdateProduct(Product product);
        public Task<ServiceResult<string>> DeleteProduct(int productId, string SellerId);
        public Task<ServiceResult<IQueryable<Product>>> GetAllProducts();
        public Task<ServiceResult<Product>> GetProductById(int id);
        public Task<ServiceResult<IQueryable<Product>>> GetProductsByUser(string userId);
        public Task<ServiceResult<IQueryable<Product>>> GetProductsByCategory(int categoryId);
    }
}
