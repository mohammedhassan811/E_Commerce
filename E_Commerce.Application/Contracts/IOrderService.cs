using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using E_Commerce.Application.Common;
using E_Commerce.Application.DTOs.Orders;

namespace E_Commerce.Application.Contracts
{
    public interface IOrderService
    {
        Task<Result<OrderToReturnDTo>> CreateOrderAsync(OrderDTo orderDTo , string email, CancellationToken ct = default);
        Task<Result<IReadOnlyList<OrderToReturnDTo>>> GetOrdersForUserAsync(string email, CancellationToken ct = default);
        Task<Result<OrderToReturnDTo>> GetOrderByIdAndEmailAsync(Guid id , string email,CancellationToken ct = default);
        Task<Result<IReadOnlyList<DeliveryMethodDTo>>> GetDeliveryMethodsAsync(CancellationToken ct = default);
    }
}
