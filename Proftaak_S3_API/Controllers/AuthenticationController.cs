using Microsoft.AspNetCore.Mvc;
using Proftaak_S3_API.Models;

namespace Proftaak_S3_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        public AuthenticationController(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        [HttpPost()]
        public string SetCookie([FromBody] UserJWT user)
        {
            CookieOptions option = new CookieOptions();
            option.HttpOnly = true;
            option.Secure = true;

            Response.Cookies.Append("jwt", user.encryptedJWT, option);
            return "Login success";
        }

        [HttpGet()]
        public string GetCookie()
        {
            return Request.Cookies["jwt"];
        }
    }
}