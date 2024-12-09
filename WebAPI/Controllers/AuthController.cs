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

        [HttpPost("register")]
        public IActionResult Register([FromBody]RegisterUserRequest request)
        {
            try
            {
                _accountService.Register(request.UserName, request.Email, request.Password);
                return NoContent();
            }
            catch
            {
                return View();
            }
        }

        [HttpPost]
        public IActionResult Login([FromBody] LoginRequest loginRequest)
        {
            try
            {
                _accountService.Login(loginRequest.Email, loginRequest.Password);
                return Ok();
            }
            catch
            {
                return View();
            }
        }

    }
}
