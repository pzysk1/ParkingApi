namespace ParkingApi.Controllers
{
    using Microsoft.AspNetCore.Mvc;
    using ParkingApi.Interfaces;

    [ApiController]
    [Route("api/authentication")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService) 
        {
            _authService = authService;
        }

        [HttpPost("token")]
        public async Task<IActionResult> GenerateTokenForUSer()
        {
            var tokenString = await _authService.GenerateToken();

            return Ok(new { Token = tokenString });
        }
    }
}
