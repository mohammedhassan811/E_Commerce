using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using E_Commerce.Domain.Entities.Products;

namespace E_Commerce.Application.Specifications
{
    internal class productWithIdSpecifications : BaseSpecifications<Product,int>
    {
        public productWithIdSpecifications(HashSet<int> productIds) : base(P=> productIds.Contains(P.Id))
        {
            
        }
    }
}
