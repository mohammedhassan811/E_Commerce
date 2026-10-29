using System.Threading.Tasks;
using E_Commerce.API.Attributes;
using E_Commerce.Application.Common;
using E_Commerce.Application.Contracts;
using E_Commerce.Application.DTOs.Products;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace E_Commerce.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ApiBaseController
    {
        private readonly IProductService _productService;

        public ProductsController(IProductService productService)
        {
            _productService = productService;
        }
        //Get all Products
        [HttpGet]
        [RedisCache(90)]
        public async Task<ActionResult<PaginatedResult<ProductDTo>>> GetAllProducts([FromQuery]ProductQueryParam queryParam, CancellationToken ct)
        {
            var result =await  _productService.GetAllProductsAsync(queryParam,ct);
            return ToActionResult(result);
        }
        //Get Product by Id
        [Authorize]
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
        public async Task<ActionResult<ProductDTo>> GetProduct(int id,CancellationToken ct)
        {
            var result = await  _productService.GetProductByIdAsync(id, ct);
            return ToActionResult(result);
        }
        //Get all Types
        [HttpGet("types")]
        public async Task<ActionResult<IReadOnlyList<TypeDTo>>> GetAllTypes(CancellationToken ct)
        {
            var result =await  _productService.GetAllTypesAsync(ct);
            return ToActionResult(result);
        }
        //Get all Brands
        [HttpGet("brands")]
        public async Task<ActionResult<IReadOnlyList<BrandDTo>>> GetAllBrands(CancellationToken ct)
        {
            var result = await _productService.GetAllBrandsAsync(ct);
            return ToActionResult(result);
        }
    }
}
