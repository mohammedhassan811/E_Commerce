using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using E_Commerce.Application.Common;
using E_Commerce.Application.DTOs.Identity;

namespace E_Commerce.Application.Contracts
{
    public interface IAuthenticationService
    {
        Task<Result<UserDto>> LoginAsync(LoginDTo loginDTo, CancellationToken ct = default);
        Task<Result<UserDto>> RegisterAsync(RegisterDTo registerDTo, CancellationToken ct = default);
        Task<Result<bool>> CheckEmailExistAsync(string email, CancellationToken ct = default);
        Task<Result<UserDto>> GetCurrentUserAsync(string email, CancellationToken ct = default);
        Task<Result<AddressDTo>> GetUserAddressAsync(string email, CancellationToken ct = default);
        Task<Result<AddressDTo>> UpsertUserAddressAsync(string email, AddressDTo addressDTo, CancellationToken ct);
    }
}
