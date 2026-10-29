using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using E_Commerce.Application.DTOs.Identity;

namespace E_Commerce.Application.DTOs.Orders
{
    public class OrderDTo
    {
        [Required]
        public string BasketId { get; set; } = default!;
        [Required]
        public int DeliveryMethodId { get; set; }
        [Required]
        public AddressDTo ShipToAddress { get; set; } = default!;
    }
}
