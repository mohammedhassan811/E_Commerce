using E_Commerce.Application.Contracts;
using E_Commerce.Application.DTOs.Baskets;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace E_Commerce.API.Controllers
{
   
    public class BasketsController : ApiBaseController
    {
        private readonly IBasketService _basketService;

        public BasketsController( IBasketService basketService)
        {
            _basketService = basketService;
        }
        // Get BaseUrl/api/Baskets/Id
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(BasketDto),StatusCodes.Status200OK)]
        public async Task<ActionResult<BasketDto>> GetBasket(string id, CancellationToken ct)
        {
            var result = await  _basketService.GetBasketAsync(id, ct);
            return ToActionResult(result);
        }
        //Post BaseUrl/api/Baskets => Body[BasketDto]
        [HttpPost]
        public async Task<ActionResult<BasketDto>> CreateOrUpdateBasket(BasketDto basket , CancellationToken ct)
        {
            var result =await  _basketService.CreateOrUpdateBasketAsync(basket, ct: ct);
            return ToActionResult(result);

        }

        // Delete BaseUrl/api/Baskets/Id
        [HttpDelete("{id}")]
        public async Task<ActionResult<bool>> DeleteBasket(string id,CancellationToken ct)
        {
           var result = await  _basketService.DeleteBasketAsync(id, ct);
            return ToActionResult(result);
        }
    }
}
