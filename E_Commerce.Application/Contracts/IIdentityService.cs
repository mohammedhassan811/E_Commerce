using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using E_Commerce.Application.Common;
using E_Commerce.Application.DTOs.Identity;

namespace E_Commerce.Application.Contracts
{
    public interface IIdentityService
    {
        Task<Result<IdentityUserResult>> FindUserByEmailAsync(string email,CancellationToken ct = default);
        Task<Result<bool>> CheckPasswordAsync(string password , string email , CancellationToken ct = default);
        Task<Result<IdentityUserResult>> CreateUserAsync(RegisterDTo registerDTo, CancellationToken ct = default);
        Task<Result<IReadOnlyList<string>>> GetUserRoles(string email,CancellationToken ct = default);
        Task<Result<bool>> EmailExistAsync(string email, CancellationToken ct = default);
        Task<Result<AddressDTo>> GetUserAddressByEmailAsync(string email, CancellationToken ct = default);
        Task<Result<AddressDTo>> UpdateOrInsertUserAddressAsync(string email, AddressDTo addressDTo, CancellationToken ct = default);
    }
}
