using AppCore.DTOs;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using AppCore.Interfaces;
using AppCore.Exceptions;

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
            try
            {
                var user = await _authService.Register(userForRegisterDto);
                return CreatedAtRoute("GetUser", new { controller = "Users", id = user.Id }, user);
            }
            catch (AlreadyExistsException)
            {
                return Ok("Username already exists");
            }
            catch
            {
                return Problem("Internal server error.");
            }
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(UserForLoginDto userForLoginDto)
        {
            try
            {
                var result = await _authService.Login(userForLoginDto);
                return Ok(result);
            }
            catch (NotFoundException)
            {
                return Unauthorized("User not found");
            }
            catch
            {
                return Problem("Internal server error.");
            }
        }
    }
}
