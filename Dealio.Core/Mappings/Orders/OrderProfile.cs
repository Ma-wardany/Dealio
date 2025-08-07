using AutoMapper;
using Dealio.Core.DTOs.Orders;
using Dealio.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dealio.Core.Mappings.Orders
{
    public partial class OrderProfile : Profile
    {
        public OrderProfile()
        {
            AddOrderMappings();
            CreateMap<Order, OrderDto>()
                .ForMember(dest => dest.OrderStatus, opt => opt.MapFrom(src => src.OrderStatus.ToString()))
                .ForMember(dest => dest.DeliveryId, opt => opt.MapFrom(src => src.DeliveryId ?? "N/A"))
                .ReverseMap();



        }
    }
}
