using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using E_Commerce.Application.Common;
using E_Commerce.Application.Contracts;
using E_Commerce.Application.DTOs.Orders;
using E_Commerce.Application.Specifications;
using E_Commerce.Domain.Contracts;
using E_Commerce.Domain.Entities.Orders;
using E_Commerce.Domain.Entities.Products;

namespace E_Commerce.Application.Services
{
    public class OrderService : IOrderService
    {
        private readonly IBasketRepository _basketRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public OrderService(IBasketRepository basketRepository ,
            IUnitOfWork unitOfWork, IMapper mapper)
        {
            _basketRepository = basketRepository;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
        public async Task<Result<OrderToReturnDTo>> CreateOrderAsync(OrderDTo orderDTo, string email, CancellationToken ct = default)
        {
            var basket =await  _basketRepository.GetBasketAsync(orderDTo.BasketId, ct);
            if (basket == null)
                return Error.NotFound("Basket Not Found", $"Basket With Id {orderDTo.BasketId} Is Not Found");
            if (basket.Items.Count == 0)
                return Error.Validation("Basket Is Empty", $"Can Not Create Order With Basket Id {orderDTo.BasketId} ");

            var exOrder =await _unitOfWork.GetRepository<Order, Guid>()
                .GetByIdAsync(new PaymentIntentSpec(basket.PaymentIntentId));
            if (exOrder != null)
                _unitOfWork.GetRepository<Order, Guid>().Remove(exOrder);
            var orderItems = new List<OrderItem>(basket.Items.Count);
            var productIds = basket.Items.Select(s => s.Id).ToHashSet();
            var products =(await  _unitOfWork.GetRepository<Product, int>()
                    .GetAllAsync(new productWithIdSpecifications(productIds),ct)).ToDictionary(x=>x.Id);
            foreach (var item in basket.Items)
            {
                if (!products.TryGetValue(item.Id, out var product))
                    return Error.NotFound("Product Not Found", $"Product With Id {item.Id} Is Not Found");
                orderItems.Add(new OrderItem()
                {
                    Price = product.Price,
                    Quantity =item.Quantity,
                    Product = new ProductItemOrdered()
                    {
                        PictureUrl =product.PictureUrl,
                        ProductId=product.Id,
                        ProductName = product.Name
                    }
                });
            }

            var orderAddress = _mapper.Map<OrderAddress>(orderDTo.ShipToAddress);

            var deliveryMethod =await  _unitOfWork.GetRepository<DeliveryMethod, int>()
                        .GetByIdAsync(orderDTo.DeliveryMethodId);
            if (deliveryMethod == null)
                return Error.NotFound("Delivery Method Is Not Found", $"Delivery Method With Id {orderDTo.DeliveryMethodId} Is Not Found");

            var subTotal = orderItems.Sum(s => s.Quantity * s.Price);
            var order = new Order(email, orderAddress, orderItems, deliveryMethod, subTotal , basket.PaymentIntentId);

            _unitOfWork.GetRepository<Order, Guid>().Add(order);
            var result =await  _unitOfWork.SaveChangesAsync(ct);
            if(result == 0)
            {
                return Error.Failure("Order Save Failed", "Can Not Create Order");
            }
            else
            {
                await _basketRepository.DeleteBasketAsync(orderDTo.BasketId, ct);
                return _mapper.Map<OrderToReturnDTo>(order);
            }
        }
        public async Task<Result<IReadOnlyList<OrderToReturnDTo>>> GetOrdersForUserAsync(string email, CancellationToken ct = default)
        {
            var orders =await _unitOfWork.GetRepository<Order, Guid>().GetAllAsync(new OrderSpecifications(email), ct);
            if (orders.Any())
            {
                return Result<IReadOnlyList<OrderToReturnDTo>>.Ok(_mapper.Map<IReadOnlyList<OrderToReturnDTo>>(orders));
            }
            else
            {
                return Error.NotFound("Orders Not Found", $"No Orders Found For user With Email {email}");
            }
        }

        public async Task<Result<OrderToReturnDTo>> GetOrderByIdAndEmailAsync(Guid id, string email, CancellationToken ct = default)
        {
            var order = await _unitOfWork.GetRepository<Order, Guid>().GetByIdAsync(new OrderSpecifications(id,email),ct);
            if(order == null)
            {
                return Error.NotFound("Order Not Found", $"Order With Id {id} Is Not Found");
            }
            else
            {
                return _mapper.Map<OrderToReturnDTo>(order);
            }
        }

        public async Task<Result<IReadOnlyList<DeliveryMethodDTo>>> GetDeliveryMethodsAsync(CancellationToken ct = default)
        {
            var deliveryMethods =await _unitOfWork.GetRepository<DeliveryMethod,int>().GetAllAsync(ct);
            if (deliveryMethods.Any())
                return Result<IReadOnlyList<DeliveryMethodDTo>>
                    .Ok(_mapper.Map<IReadOnlyList<DeliveryMethodDTo>>(deliveryMethods));
            else
                return Error.NotFound("No Delivery Methods Found");
        }
    }
}
