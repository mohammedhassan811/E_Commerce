using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using E_Commerce.Application.DTOs.Identity;

namespace E_Commerce.Application.DTOs.Orders
{
    public class OrderToReturnDTo
    {
        public Guid Id { get; set; }
        public string BuyerEmail { get; set; } = default!;
        public DateTimeOffset OrderDate { get; set; }
        public AddressDTo ShipToAddress { get; set; }
        public string DeliveryMethod { get; set; } = default!;
        public string Status { get; set; } = default!;
        public decimal SubTotal { get; set; }
        public decimal Total { get; set; }
        public decimal DeliveryMethodCost { get; set; }
        public ICollection<OrderItemDTo> Items { get; set; }
    }
}
