using Dealio.Domain.Entities;
using Dealio.Infrastructure.Repositories.Interfaces;
using Dealio.Services.Helpers;
using Dealio.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Dealio.Services.ServicesImp
{
    public class ProductServices : IProductServices
    {
        private readonly IProductRepository productRepository;
        private readonly ICategoryRepository categoryRepository;
        private readonly IOrderRepository orderRepository;

        public ProductServices(
            IProductRepository productRepository,
            ICategoryRepository categoryRepository,
            IOrderRepository orderRepository)
        {
            this.productRepository = productRepository;
            this.categoryRepository = categoryRepository;
            this.orderRepository = orderRepository;
        }

        public async Task<ServiceResult<Product>> CreateProduct(Product product)
        {
            var category = await categoryRepository.GetByIdAsync(product.CategoryId);
            if (category == null)
                ServiceResult<Product>.Failure(ServiceResultEnum.NotFound);

            try
            {
                await productRepository.AddAsync(product);
                return ServiceResult<Product>.Success(product, ServiceResultEnum.Created);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.InnerException + "-----------------------------");
                return ServiceResult<Product>.Failure(ServiceResultEnum.Failed);
            }

        }

        public async Task<ServiceResult<string>> DeleteProduct(int productId, string SellerId)
        {
            var existProduct = await productRepository.GetByIdAsync(productId);
            if (existProduct == null)
                return ServiceResult<string>.Failure(ServiceResultEnum.NotFound);

            if (existProduct.SellerId != SellerId)
                return ServiceResult<string>.Failure(ServiceResultEnum.NoAccess);

            var ordered = await orderRepository.GetTableNoTracking()
                .AnyAsync(o => o.ProductId == productId);

            if (ordered)
                return ServiceResult<string>.Failure(ServiceResultEnum.Ordered);

            try
            {
                await productRepository.DeleteAsync(existProduct);
                return ServiceResult<string>.Success("deleted successfully!", ServiceResultEnum.Deleted);
            }
            catch (Exception ex)
            {
                return ServiceResult<string>.Failure(ServiceResultEnum.Failed);
            }
        }

        public async Task<ServiceResult<IQueryable<Product>>> GetAllProducts()
        {
            var products = productRepository.GetTableNoTracking().Include(p => p.Images);
            if (!products.Any())
                return ServiceResult<IQueryable<Product>>.Failure(ServiceResultEnum.Empty);

            return ServiceResult<IQueryable<Product>>.Success(products, ServiceResultEnum.Success);
        }

        public async Task<ServiceResult<Product>> GetProductById(int id)
        {
            var product = await productRepository.GetTableNoTracking()
                                                 .Include(p => p.Images)
                                                 .SingleOrDefaultAsync(p => p.Id == id);

            if (product == null)
                return ServiceResult<Product>.Failure(ServiceResultEnum.NotFound);

            return ServiceResult<Product>.Success(product, ServiceResultEnum.Success);
        }

        public async Task<ServiceResult<IQueryable<Product>>> GetProductsByCategory(int categoryId)
        {
            return await GetFilteredProducts(p => p.CategoryId == categoryId);
        }

        public async Task<ServiceResult<IQueryable<Product>>> GetProductsByUser(string userId)
        {
            return await GetFilteredProducts(p => p.SellerId == userId);
        }

        public async Task<ServiceResult<Product>> UpdateProduct(Product product)
        {
            var existProduct = await productRepository.GetByIdAsync(product.Id);
            if (existProduct == null)
                return ServiceResult<Product>.Failure(ServiceResultEnum.NotFound);

            if (existProduct.SellerId != product.SellerId)
                return ServiceResult<Product>.Failure(ServiceResultEnum.NoAccess);

            var ordered = await orderRepository.GetTableNoTracking()
                                               .AnyAsync(o => o.ProductId == product.Id);
            if (ordered)
                return ServiceResult<Product>.Failure(ServiceResultEnum.Ordered);


            try
            {
                existProduct.Name        = product.Name;
                existProduct.Description = product.Description;
                existProduct.CategoryId  = product.CategoryId;
                existProduct.Price       = product.Price;
                existProduct.SellerId    = product.SellerId;

                await productRepository.UpdateAsync(existProduct);
                return ServiceResult<Product>.Success(existProduct, ServiceResultEnum.Updated);
            }
            catch (Exception ex)
            {
                return ServiceResult<Product>.Failure(ServiceResultEnum.Failed);
            }
        }



        private async Task<ServiceResult<IQueryable<Product>>> GetFilteredProducts(Expression<Func<Product, bool>> filter)
        {
            var products = productRepository.GetTableNoTracking()
                                            .Include(p => p.Images)
                                            .Where(filter);

            if (!products.Any())
                return ServiceResult<IQueryable<Product>>.Failure(ServiceResultEnum.Empty);

            return ServiceResult<IQueryable<Product>>.Success(products, ServiceResultEnum.Success);
        }
    }
}
