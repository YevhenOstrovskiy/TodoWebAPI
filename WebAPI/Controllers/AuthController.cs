using Businesslogic;
using BusinessLogic;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers
{
        [ApiController]
    [Route("[controller]")]
    public class AuthController : Controller
    {
        private readonly IAccountService _accountService;

        public AuthController(IAccountService accountService)
        {
            _accountService = accountService;
        }

        [HttpPost("register")]
        public IActionResult Register([FromBody]RegisterUserRequest request)
        {
                _accountService.Register(request.UserName, request.Email, request.Password);
                return NoContent();
        }

        [HttpPost("login")]
        public IActionResult Login([FromBody] LoginRequest loginRequest)
        {

                var token = _accountService.Login(loginRequest.Email, loginRequest.Password);
                return Ok(token);
        }

    }
}
