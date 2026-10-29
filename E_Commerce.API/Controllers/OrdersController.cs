using E_Commerce.Application.Contracts;
using E_Commerce.Application.DTOs.Orders;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace E_Commerce.API.Controllers
{
   
    public class OrdersController : ApiBaseController
    {
        private readonly IOrderService _orderService;

        public OrdersController(IOrderService orderService)
        {
            _orderService = orderService;
        }
        //Post Create Order
        [Authorize]
        [HttpPost]
        public async Task<ActionResult<OrderToReturnDTo>> CreateOrder(OrderDTo orderDTo , CancellationToken ct)
        {
            return ToActionResult( await _orderService.CreateOrderAsync(orderDTo, GetEmailFromToken(), ct));
        }
        //Get Orders of User
        [Authorize]
        [HttpGet]
        public async Task<ActionResult<IReadOnlyList<OrderToReturnDTo>>> GetAllOrders(CancellationToken ct)
        {
            return ToActionResult( await _orderService.GetOrdersForUserAsync(GetEmailFromToken(), ct));
        }
        //Get Order of User
        [Authorize]
        [HttpGet("{id:guid}")]
        public async Task<ActionResult<OrderToReturnDTo>> GetOrderById(Guid id , CancellationToken ct)
        {
            return ToActionResult(await _orderService.GetOrderByIdAndEmailAsync(id, GetEmailFromToken(), ct));
        }
        //Get Delivery Methods
        [AllowAnonymous]
        [HttpGet("DeliveryMethod")]
        public async Task<ActionResult<IReadOnlyList<DeliveryMethodDTo>>> GetDeliveryMethods(CancellationToken ct)
        {
            return ToActionResult(await _orderService.GetDeliveryMethodsAsync(ct));
        }

    }
}
