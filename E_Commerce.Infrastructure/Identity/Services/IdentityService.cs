using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using E_Commerce.Application.Common;
using E_Commerce.Application.Contracts;
using E_Commerce.Application.DTOs.Identity;
using E_Commerce.Infrastructure.Identity.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace E_Commerce.Infrastructure.Identity.Services
{
    internal class IdentityService : IIdentityService
    {
        private readonly UserManager<ApplicationUser> _userManager;

        public IdentityService(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }
        public async Task<Result<bool>> CheckPasswordAsync(string password, string email, CancellationToken ct = default)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user == null)
                return Result<bool>.Fail(Error.NotFound("User Not Found", $"Usr With Email {email} Not Found"));
            else
                return await _userManager.CheckPasswordAsync(user, password);

        }

        public async Task<Result<IdentityUserResult>> CreateUserAsync(RegisterDTo registerDTo, CancellationToken ct = default)
        {
            var user = new ApplicationUser()
            {
                Email = registerDTo.Email,
                UserName = registerDTo.UserName,
                DisplayName = registerDTo.DisplayName,
                PhoneNumber = registerDTo.PhoneNumber
            };
            var result = await _userManager.CreateAsync(user, registerDTo.Password);
            if (!result.Succeeded)
            {
                var errors = result.Errors.Select(e => new Error(e.Code, e.Description)).ToList();
                return Result<IdentityUserResult>.Fail(errors);
            }
            return Result<IdentityUserResult>.Ok(new IdentityUserResult(user.Id, user.DisplayName, user.Email, user.UserName));
        }

        public async Task<Result<bool>> EmailExistAsync(string email, CancellationToken ct = default)
        {
            return await _userManager.FindByEmailAsync(email) is not null;
        }

        public async Task<Result<IdentityUserResult>> FindUserByEmailAsync(string email, CancellationToken ct = default)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user == null)
                return Result<IdentityUserResult>.Fail(Error.NotFound("User Not Found", $"Usr With Email {email} Not Found"));
            else
                return Result<IdentityUserResult>.Ok(new IdentityUserResult(user.Id, user.DisplayName, user.Email, user.UserName));
        }

        public async Task<Result<IReadOnlyList<string>>> GetUserRoles(string email, CancellationToken ct = default)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user == null)
                return Error.NotFound("User Not Found", $"User With Email {email} Is not Found");
            var roles = await _userManager.GetRolesAsync(user);
            return roles.ToList();

        }
        public async Task<Result<AddressDTo>> GetUserAddressByEmailAsync(string email, CancellationToken ct = default)
        {
            var user = await _userManager.Users.Include(x => x.Address).FirstOrDefaultAsync(x => x.Email == email, ct);
            if (user.Address == null)
                return Result<AddressDTo>.Fail(Error.NotFound("Address not Found", $"Address with email {email} is not Found"));
            var address = user.Address;
            return new AddressDTo()
            {
                FirstName = address.FirstName,
                LastName = address.LastName,
                City = address.City,
                Street = address.Street,
                Country = address.Country
            };
        }

        public async Task<Result<AddressDTo>> UpdateOrInsertUserAddressAsync(string email, AddressDTo addressDTo, CancellationToken ct = default)
        {
            var user = await _userManager.Users.Include(x => x.Address).FirstOrDefaultAsync(x => x.Email == email, ct);
            if (user?.Address == null)
            {
                user.Address = new Address()
                {
                    FirstName = addressDTo.FirstName,
                    LastName = addressDTo.LastName,
                    City = addressDTo.City,
                    Country = addressDTo.Country,
                    Street = addressDTo.Street,
                };
            }
            else
            {
                user.Address.FirstName = addressDTo.FirstName;
                user.Address.LastName = addressDTo.LastName;
                user.Address.City = addressDTo.City;
                user.Address.Country = addressDTo.Country;
                user.Address.Street = addressDTo.Street;
            }
            var result = await _userManager.UpdateAsync(user);
            if (result.Succeeded)
                return addressDTo;
            else
                return Error.Failure("Failure", string.Join(';', result.Errors.Select(e => e.Description)));
        }
    }
}
