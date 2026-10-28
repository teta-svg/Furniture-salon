using FurnitureSalon.Api.Interfaces;
using FurnitureSalon.Core.DTO.Login;
using Microsoft.AspNetCore.Mvc;

namespace FurnitureSalon.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest request)
        {
            var response = await _authService.LoginAsync(request);

            if (response == null)
            {
                return Unauthorized();
            }

            return Ok(response);
        }
    }
}