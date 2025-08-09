using AutoMapper;
using Dealio.Core.Bases;
using Dealio.Core.DTOs.Product;
using Dealio.Core.Features.Product.Commands.Models;
using Dealio.Domain.Entities;
using Dealio.Services.Helpers;
using Dealio.Services.Interfaces;
using MediatR;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;

namespace Dealio.Core.Features.Product.Commands.Handler
{
    public class ProductRequestHandler : IRequestHandler<AddProductCommand, Response<ProductDto>>,
                                         IRequestHandler<UpdateProductCommand, Response<ProductDto>>,
                                         IRequestHandler<DeleteProductCommand, Response<string>>
    {
        private readonly IMapper mapper;
        private readonly IProductServices productServices;
        private readonly IWebHostEnvironment webHostEnvironment;
        private readonly IHttpContextAccessor httpContextAccessor;

        public ProductRequestHandler(
            IMapper mapper,
            IProductServices productServices,
            IWebHostEnvironment webHostEnvironment,
            IHttpContextAccessor httpContextAccessor)
        {
            this.mapper = mapper;
            this.productServices = productServices;
            this.webHostEnvironment = webHostEnvironment;
            this.httpContextAccessor = httpContextAccessor;
        }

        public async Task<Response<ProductDto>> Handle(AddProductCommand request, CancellationToken cancellationToken)
        {
            string ImageUploadDirectory = Path.Combine(webHostEnvironment.WebRootPath ?? throw new InvalidOperationException("WebRootPath is null"), "Images");
            Directory.CreateDirectory(ImageUploadDirectory);

            List<ProductImage> ImagePathes = new();

            foreach(var image in request.Images)
            {
                var imageName = $"{Guid.NewGuid()}{Path.GetExtension(image.FileName)}";
                var imageFolder = Path.Combine(webHostEnvironment.WebRootPath, "Images");
                var imagePath = Path.Combine(imageFolder, imageName);

                // Ensure directory exists
                if (!Directory.Exists(imageFolder))
                    Directory.CreateDirectory(imageFolder);

                using (var stream = new FileStream(imagePath, FileMode.Create))
                {
                    await image.CopyToAsync(stream);
                }
                var baseUrl = $"{httpContextAccessor.HttpContext.Request.Scheme}://{httpContextAccessor.HttpContext.Request.Host}";

                ImagePathes.Add(new ProductImage
                {
                    ImgUrl = $"{baseUrl}/Images/{imageName}"
                });
            }


            var mappedProduct = mapper.Map<Domain.Entities.Product>(request);
            mappedProduct.Images = ImagePathes;

            var result = await productServices.CreateProduct(mappedProduct);
            var product = mapper.Map<ProductDto>(result.Data);
            var status = result.ResultEnum;

            return status switch
            {
                ServiceResultEnum.Created => Response<ProductDto>.Created(product, "Product created successfully"),
                ServiceResultEnum.NotFound => Response<ProductDto>.NotFound("Category not found"),
                ServiceResultEnum.Failed => Response<ProductDto>.BadRequest("Something went wrong!"),
                _ => Response<ProductDto>.BadRequest("An unexpected error occurred")
            };
        }

        public async Task<Response<ProductDto>> Handle(UpdateProductCommand request, CancellationToken cancellationToken)
        {
            var mappedProduct = mapper.Map<Domain.Entities.Product>(request);

            var result  = await productServices.UpdateProduct(mappedProduct);
            var product = mapper.Map<ProductDto>(result.Data);
            var status  = result.ResultEnum;


            return status switch
            {
                ServiceResultEnum.Updated  => Response<ProductDto>.Success(product, "Product updated successfully"),
                ServiceResultEnum.NotFound => Response<ProductDto>.NotFound("Product not found"),
                ServiceResultEnum.Ordered  => Response<ProductDto>.BadRequest("product in order"),
                ServiceResultEnum.NoAccess => Response<ProductDto>.Unauthorized("Product can't be accessed by this user"),
                ServiceResultEnum.Failed   => Response<ProductDto>.BadRequest("Something went wrong!"),
                _ => Response<ProductDto>.BadRequest("An unexpected error occurred")
            };
        }

        public async Task<Response<string>> Handle(DeleteProductCommand request, CancellationToken cancellationToken)
        {
            var result = await productServices.DeleteProduct(request.ProductId, request.SellerId);
            var status = result.ResultEnum;
            return status switch
            {
                ServiceResultEnum.Deleted     => Response<string>.Success("Product deleted successfully"),
                ServiceResultEnum.NotFound    => Response<string>.NotFound("Product not found"),
                ServiceResultEnum.NoAccess    => Response<string>.Unauthorized("Product can't be accessed by this user"),
                ServiceResultEnum.Ordered     => Response<string>.BadRequest("Product is in an order and cannot be deleted"),
                _ or ServiceResultEnum.Failed => Response<string>.BadRequest("Something went wrong!")
            };
        }
    }
}
