using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using E_Commerce.Application.Common;
using E_Commerce.Application.Contracts;
using E_Commerce.Application.DTOs.Baskets;
using E_Commerce.Domain.Contracts;
using E_Commerce.Domain.Entities.Baskets;

namespace E_Commerce.Application.Services
{
    internal class BasketService : IBasketService
    {
        private readonly IBasketRepository _basketRepository;
        private readonly IMapper _mapper;

        public BasketService(IBasketRepository basketRepository,IMapper mapper)
        {
            _basketRepository = basketRepository;
            _mapper = mapper;
        }
        public async Task<Result<BasketDto>> CreateOrUpdateBasketAsync(BasketDto basket, TimeSpan? TTL = null, CancellationToken ct = default)
        {
           var customerBasket = _mapper.Map<CustomerBasket>(basket);
           var basketResult = await  _basketRepository.CreateOrUpdateBasketAsync(customerBasket, TTL, ct);
            return basketResult == null ? Result<BasketDto>.Fail(Error.Failure("BasketCreate.Failure", "Can Not Create or Update Basket "))
                : Result<BasketDto>.Ok(basket);
        }

        public async Task<Result<bool>> DeleteBasketAsync(string basketId, CancellationToken ct = default)
        {
            var result = await _basketRepository.DeleteBasketAsync(basketId, ct);
            return result ? Result<bool>.Ok(true)
                : Result<bool>.Fail(Error.Failure("BasketDelete.Failure", "Can Not Delete Basket"));
        }

        public async Task<Result<BasketDto>> GetBasketAsync(string BasketId, CancellationToken ct = default)
        {
           var result = await  _basketRepository.GetBasketAsync(BasketId, ct);
            return result == null ? Result<BasketDto>.Fail(Error.NotFound("Basket Not Found")) : _mapper.Map<BasketDto>(result);
        }
    }
}
