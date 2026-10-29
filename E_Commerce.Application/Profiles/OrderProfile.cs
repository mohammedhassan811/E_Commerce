using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using E_Commerce.Application.DTOs.Identity;
using E_Commerce.Application.DTOs.Orders;
using E_Commerce.Domain.Entities.Orders;

namespace E_Commerce.Application.Profiles
{
    internal class OrderProfile : Profile
    {
        public OrderProfile()
        {
            CreateMap<AddressDTo, OrderAddress>().ReverseMap();

            CreateMap<Order, OrderToReturnDTo>()
                .ForMember(x => x.DeliveryMethod, o => o.MapFrom(d => d.DeliveryMethod.ShortName))
                .ForMember(x => x.DeliveryMethodCost, o => o.MapFrom(d => d.DeliveryMethod.Cost));

            CreateMap<OrderItem, OrderItemDTo>()
                .ForMember(x => x.ProductId, o => o.MapFrom(x => x.Product.ProductId))
                .ForMember(x => x.ProductName, o => o.MapFrom(x => x.Product.ProductName))
                .ForMember(x => x.PictureUrl, o => o.MapFrom<OrderItemPictureUrlResolver>());

            CreateMap<DeliveryMethod, DeliveryMethodDTo>();
        }
    }
}
