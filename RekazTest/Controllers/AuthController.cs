using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RekazTest.Security;

namespace RekazTest.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : Controller
    {
        private readonly IAuth _auth;
        public AuthController(IAuth auth)
        {
            _auth = auth;
        }

        [AllowAnonymous]
        [HttpGet]
        public IActionResult GenerateAuthToken()
        {
            var response = _auth.AuthorizeRequest();

            return Ok(response);
        }
    }
}
