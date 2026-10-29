using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using E_Commerce.Application.Common;
using E_Commerce.Application.Contracts;
using E_Commerce.Application.DTOs.Identity;

namespace E_Commerce.Application.Services
{
    public class AuthenticationService : IAuthenticationService
    {
        private readonly IIdentityService _identityService;
        private readonly ITokenService _tokenService;

        public AuthenticationService(IIdentityService identityService , ITokenService tokenService)
        {
            _identityService = identityService;
            _tokenService = tokenService;
        }

        public async Task<Result<bool>> CheckEmailExistAsync(string email, CancellationToken ct = default)
         => await  _identityService.EmailExistAsync(email, ct);

        public async Task<Result<UserDto>> GetCurrentUserAsync(string email, CancellationToken ct = default)
        {
            var userResult =await  _identityService.FindUserByEmailAsync(email, ct);
            var user = userResult.data;
            var role =await  _identityService.GetUserRoles(email, ct);
            var token =_tokenService.CreateToken(user.Id, user.Email, user.UserName, role.data);
            return new UserDto() { DisplayName = user.DisplayName, Email = user.Email, Token = token };
        }

        public async Task<Result<AddressDTo>> GetUserAddressAsync(string email, CancellationToken ct = default)
        {
            return await _identityService.GetUserAddressByEmailAsync(email, ct);
        }

        public async Task<Result<UserDto>> LoginAsync(LoginDTo loginDTo, CancellationToken ct = default)
        {
            //Get User By Email
            var userResult =await _identityService.FindUserByEmailAsync(loginDTo.Email);
            if (!userResult.IsSuccess)
                return Result<UserDto>.Fail(userResult.Errors);
            //Check Password
            var passwordResult =await  _identityService.CheckPasswordAsync(loginDTo.Password, loginDTo.Email, ct);
            if(!passwordResult.IsSuccess)
                return Result<UserDto>.Fail(userResult.Errors);
            if (!passwordResult.data)
                return Result<UserDto>.Fail(Error.Unauthorized("Invalid Email or Password"));
            var user = userResult.data;
            var rolesResult =await  _identityService.GetUserRoles(user.Email);
            var roles = rolesResult.data;
            var token = _tokenService.CreateToken(user.Id, user.Email, user.UserName, roles);
            return new UserDto()
            {
                Email = loginDTo.Email,
                DisplayName = userResult.data.DisplayName,
                Token = token
            };

            
        }

        public async Task<Result<UserDto>> RegisterAsync(RegisterDTo registerDTo, CancellationToken ct = default)
        {
            var userResult =await  _identityService.CreateUserAsync(registerDTo, ct);
            if (!userResult.IsSuccess)
            {
                return Result<UserDto>.Fail(userResult.Errors);
            }
            var user = userResult.data;
            var rolesResult = await _identityService.GetUserRoles(user.Email);
            var roles = rolesResult.data;
            var token = _tokenService.CreateToken(user.Id, user.Email, user.UserName, roles);
            return Result<UserDto>.Ok(new UserDto
            {
                Email = user.Email,
                DisplayName = user.DisplayName,
                Token = token
            });
        }

        public async Task<Result<AddressDTo>> UpsertUserAddressAsync(string email, AddressDTo addressDTo, CancellationToken ct)
        {
            return await _identityService.UpdateOrInsertUserAddressAsync(email, addressDTo, ct);
        }
    }
}
