using AutoMapper;
using Dealio.Core.Bases;
using Dealio.Core.DTOs.Orders;
using Dealio.Core.Features.Orders.Commands.Models;
using Dealio.Domain.Entities;
using Dealio.Services.Helpers;
using Dealio.Services.Interfaces;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dealio.Core.Features.Orders.Commands.Handler
{
    public class OrderCommandsHandler : IRequestHandler<AddOrderCommand, Response<OrderDto>>,
                                          IRequestHandler<DeleteOrderCommand, Response<string>>
    {
        private readonly IMapper mapper;
        private readonly IOrderServices orderServices;

        public OrderCommandsHandler(IMapper mapper, IOrderServices orderServices)
        {
            this.mapper = mapper;
            this.orderServices = orderServices;
        }

        public async Task<Response<OrderDto>> Handle(AddOrderCommand request, CancellationToken cancellationToken)
        {
            var order = mapper.Map<Order>(request);
            var result = await orderServices.CreateOrder(order);

            var  status = result.ResultEnum;
            if (status == ServiceResultEnum.Created)
            {
                var orderDto = mapper.Map<OrderDto>(result.Data);
                return Response<OrderDto>.Created(orderDto, "Order created successfully");
            }
            
            return status switch
            {
                ServiceResultEnum.NotFound => Response<OrderDto>.NotFound("Product not found"),
                ServiceResultEnum.SameSellerAndBuyer => Response<OrderDto>.BadRequest("Seller and buyer cannot be the same"),
                ServiceResultEnum.Failed => Response<OrderDto>.BadRequest("Failed to create order"),
                _ => Response<OrderDto>.BadRequest("An unexpected error occurred")
            };

        }

        public async Task<Response<string>> Handle(DeleteOrderCommand request, CancellationToken cancellationToken)
        {
            var result = await orderServices.DeleteOrder(request.OrderId, request.BuyerId);
            var status = result.ResultEnum;
            
            return status switch
            {
                ServiceResultEnum.Deleted  => Response<string>.Success("Order deleted successfully"),
                ServiceResultEnum.NotFound => Response<string>.NotFound("Order not found"),
                ServiceResultEnum.NoAccess => Response<string>.Forbidden("You do not have access to delete this order"),
                ServiceResultEnum.Failed   =>   Response<string>.BadRequest("Failed to delete order"),
                _ => Response<string>.BadRequest("An unexpected error occurred")
            };
        }
    }
}
