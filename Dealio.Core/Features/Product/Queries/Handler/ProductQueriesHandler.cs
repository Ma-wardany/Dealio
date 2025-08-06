using AutoMapper;
using Dealio.Core.Bases;
using Dealio.Core.DTOs.Product;
using Dealio.Core.Features.Product.Queries.Models;
using Dealio.Services.Helpers;
using Dealio.Services.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Dealio.Core.Features.Product.Queries.Handler
{
    public class ProductQueriesHandler : IRequestHandler<GetAllProductsQuery, Response<List<ProductDto>>>,
                                         IRequestHandler<GetProductsByCategoryQuery, Response<List<ProductDto>>>,
                                         IRequestHandler<GetProductsByUserQuery, Response<List<ProductDto>>>,
                                         IRequestHandler<GetProductByIdQuery, Response<ProductDto>>
    {
        private readonly IMapper mapper;
        private readonly IProductServices productServices;

        public ProductQueriesHandler(IMapper mapper, IProductServices productServices)
        {
            this.mapper = mapper;
            this.productServices = productServices;
        }

        public async Task<Response<List<ProductDto>>> Handle(GetAllProductsQuery request, CancellationToken cancellationToken)
        {
            var result = await productServices.GetAllProducts();

            return await GetProductsHandler(result);
        }

        public async Task<Response<List<ProductDto>>> Handle(GetProductsByCategoryQuery request, CancellationToken cancellationToken)
        {
            var result = await productServices.GetProductsByCategory(request.CategoryId);
            
            return await GetProductsHandler(result);
        }

        public async Task<Response<List<ProductDto>>> Handle(GetProductsByUserQuery request, CancellationToken cancellationToken)
        {
            var result = await productServices.GetProductsByUser(request.UserId);

            return await GetProductsHandler(result);
        }

        public async Task<Response<ProductDto>> Handle(GetProductByIdQuery request, CancellationToken cancellationToken)
        {
            var result = await productServices.GetProductById(request.Id);
            var status = result.ResultEnum;
            var product = mapper.Map<ProductDto>(result.Data);

            return status switch
            {
                ServiceResultEnum.Success => Response<ProductDto>.Success(product, "Product retrieved successfully"),
                ServiceResultEnum.NotFound => Response<ProductDto>.NotFound("Product not found"),
                _ => Response<ProductDto>.BadRequest("Something went wrong!"),
            };
        }

        private async Task<Response<List<ProductDto>>> GetProductsHandler(ServiceResult<IQueryable<Domain.Entities.Product>> result)
        {
            var status = result.ResultEnum;
            var products = await mapper.ProjectTo<ProductDto>(result.Data).ToListAsync();

            return status switch
            {
                ServiceResultEnum.Success => Response<List<ProductDto>>.Success(products, "Products retrieved successfully"),
                ServiceResultEnum.Empty => Response<List<ProductDto>>.NotFound("empty products"),
                _ => Response<List<ProductDto>>.BadRequest("Something went wrong!"),
            };
        }
    }
}
