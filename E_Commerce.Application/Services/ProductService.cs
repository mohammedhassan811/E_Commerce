using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using E_Commerce.Application.Common;
using E_Commerce.Application.Contracts;
using E_Commerce.Application.DTOs.Products;
using E_Commerce.Application.Specifications;
using E_Commerce.Domain.Contracts;
using E_Commerce.Domain.Entities.Products;

namespace E_Commerce.Application.Services
{
    internal class ProductService : IProductService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public ProductService(IUnitOfWork unitOfWork,IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
        public async Task<Result<IReadOnlyList<BrandDTo>>> GetAllBrandsAsync(CancellationToken ct = default)
        {
            var brands =await  _unitOfWork.GetRepository<ProductBrand,int>().GetAllAsync(ct);
            var data = _mapper.Map<IReadOnlyList<BrandDTo>>(brands);
            return Result<IReadOnlyList<BrandDTo>>.Ok(data);
        }

        public async Task<Result<PaginatedResult<ProductDTo>>> GetAllProductsAsync(ProductQueryParam queryParam , CancellationToken ct = default)
        {
            var spec = new ProductWithTypeAndBrandSpec(queryParam);
            var products =await  _unitOfWork.GetRepository<Product, int>().GetAllAsync(spec);
            var data = _mapper.Map<IReadOnlyList<ProductDTo>>(products);
            var countSpec = new ProductCountSpecifications(queryParam);
            var countOfAllProducts = await  _unitOfWork.GetRepository<Product,int>().GetCountAsync(countSpec);
            var result = new PaginatedResult<ProductDTo>(queryParam.PageIndex, queryParam.PageSize, countOfAllProducts, data);
            return Result<PaginatedResult<ProductDTo>>.Ok(result);
        }

        public async Task<Result<IReadOnlyList<TypeDTo>>> GetAllTypesAsync(CancellationToken ct = default)
        {
            var types =await  _unitOfWork.GetRepository<ProductType, int>().GetAllAsync(ct);
            var data = _mapper.Map<IReadOnlyList<TypeDTo>>(types);
            return Result<IReadOnlyList<TypeDTo>>.Ok(data);
        }

        public async Task<Result<ProductDTo>> GetProductByIdAsync(int id, CancellationToken ct = default)
        {
            var spec = new ProductWithTypeAndBrandSpec(id);
            var product =await  _unitOfWork.GetRepository<Product,int>().GetByIdAsync(spec, ct);
            if (product == null)
                return Error.NotFound("Product Not Found", $"Product with Id {id} is Not Found");
            return _mapper.Map<ProductDTo>(product);
        }
    }
}
