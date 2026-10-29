using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using E_Commerce.Application.Common;
using E_Commerce.Domain.Entities.Products;

namespace E_Commerce.Application.Specifications
{
    internal class ProductCountSpecifications : BaseSpecifications<Product,int>
    {
        public ProductCountSpecifications(ProductQueryParam queryParam)
            : base(P => (!queryParam.BrandId.HasValue || P.BrandId == queryParam.BrandId.Value)
            && (!queryParam.TypeId.HasValue || P.TypeId == queryParam.TypeId.Value)
            && (string.IsNullOrWhiteSpace(queryParam.searchValue) || P.Name.ToLower().Contains(queryParam.searchValue.ToLower())))
        {
            
        }
    }
}
