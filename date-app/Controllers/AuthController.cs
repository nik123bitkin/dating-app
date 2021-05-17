using System.Threading.Tasks;
using AppCore.DTOs;
using AppCore.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace date_app.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] UserForRegisterDto userForRegisterDto)
        {
            var user = await _authService.RegisterAsync(userForRegisterDto);
            return CreatedAtRoute("GetUser", new { controller = "Users", id = user.Id }, user);
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(UserForLoginDto userForLoginDto)
        {
            var result = await _authService.LoginAsync(userForLoginDto);
            return Ok(result);
        }
    }
}
