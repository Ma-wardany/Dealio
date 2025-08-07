using AutoMapper;
using Dealio.Core.Features.Orders.Commands.Models;
using Dealio.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dealio.Core.Mappings.Orders
{
    public partial class OrderProfile
    {
        public void AddOrderMappings()
        {
            CreateMap<AddOrderCommand, Order>()
                .ForMember(dest => dest.OrderDate, opt => opt.MapFrom(src => DateTime.UtcNow))
                .ForMember(dest => dest.OrderNumber, opt => opt.MapFrom(src => new Random().Next(100000, 999999).ToString()))
                .ForMember(dest => dest.OrderStatus, opt => opt.MapFrom(src => Domain.Enums.OrderStatus.Pending))
                .ForMember(dest => dest.DeliveryId, opt => opt.MapFrom(src => (string?)null));
        }
    }
}
