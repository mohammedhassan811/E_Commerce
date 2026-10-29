using System.Security.Claims;
using E_Commerce.Application.Contracts;
using E_Commerce.Application.DTOs.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace E_Commerce.API.Controllers
{
   
    public class AuthenticationController : ApiBaseController
    {
        private readonly IAuthenticationService _authenticationService;

        public AuthenticationController(IAuthenticationService authenticationService)
        {
            _authenticationService = authenticationService;
        }
        [HttpPost("Login")]
        [ProducesResponseType(typeof(UserDto),StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<UserDto>> Login(LoginDTo loginDTo)
         => ToActionResult(await _authenticationService.LoginAsync(loginDTo));

        [HttpPost("register")]
        public async Task<ActionResult<UserDto>> Register(RegisterDTo registerDTo,CancellationToken ct = default)
        {
            return ToActionResult(await  _authenticationService.RegisterAsync(registerDTo, ct));
        }

        [HttpGet("emailexists")]
        public async Task<ActionResult<bool>> CheckEmail( [FromQuery]string email , CancellationToken ct)
            => ToActionResult(await _authenticationService.CheckEmailExistAsync(email, ct));

        [HttpGet("currentUser")]
        public async Task<ActionResult<UserDto>> GetCurrentUser(CancellationToken ct)
            => ToActionResult(await _authenticationService.GetCurrentUserAsync(GetEmailFromToken(), ct));
        
        [Authorize]
        [HttpGet("address")]
        public async Task<ActionResult<AddressDTo>> GetCurrentUserAddress(CancellationToken ct)
            => ToActionResult(await  _authenticationService.GetUserAddressAsync(GetEmailFromToken(), ct));

        [Authorize]
        [HttpPut("address")]
        public async Task<ActionResult<AddressDTo>> UpdateUserAddress(AddressDTo addressDTo , CancellationToken ct)
            => ToActionResult(await _authenticationService.UpsertUserAddressAsync(GetEmailFromToken(),addressDTo , ct));
        


        
    }
}
