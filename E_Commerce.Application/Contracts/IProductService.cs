using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using E_Commerce.Application.Common;
using E_Commerce.Application.DTOs.Products;

namespace E_Commerce.Application.Contracts
{
    public interface IProductService
    {
        Task<Result<PaginatedResult<ProductDTo>>> GetAllProductsAsync(ProductQueryParam queryParam,CancellationToken ct = default);
        Task<Result<IReadOnlyList<BrandDTo>>> GetAllBrandsAsync(CancellationToken ct = default);
        Task<Result<IReadOnlyList<TypeDTo>>> GetAllTypesAsync(CancellationToken ct = default);
        Task<Result<ProductDTo>> GetProductByIdAsync(int id, CancellationToken ct = default);
    }
}
