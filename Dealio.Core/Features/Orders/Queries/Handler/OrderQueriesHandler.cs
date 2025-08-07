using AutoMapper;
using Dealio.Core.Bases;
using Dealio.Core.DTOs.Orders;
using Dealio.Core.Features.Orders.Queries.Models;
using Dealio.Services.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dealio.Core.Features.Orders.Queries.Handler
{
    public class OrderQueriesHandler : IRequestHandler<GetBuyerOrdersQuery, Response<List<OrderDto>>>
    {
        private readonly IMapper mapper;
        private readonly IOrderServices orderServices;

        public OrderQueriesHandler(IMapper mapper, IOrderServices orderServices)
        {
            this.mapper = mapper;
            this.orderServices = orderServices;
        }

        public async Task<Response<List<OrderDto>>> Handle(GetBuyerOrdersQuery request, CancellationToken cancellationToken)
        {
            var orders = await orderServices.GetBuyerOrders(request.BuyerId);
            var status = orders.ResultEnum;

            if (orders.Data.Any())
            {
                var orderDtos = await mapper.ProjectTo<OrderDto>(orders.Data).ToListAsync();
                return Response<List<OrderDto>>.Success(orderDtos, "Orders retrieved successfully");
            }

            return Response<List<OrderDto>>.NotFound("No orders found for this buyer");
        }
    }
}
