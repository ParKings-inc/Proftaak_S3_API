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
        public IActionResult SetCookie([FromBody] UserJWT user)
        {
            CookieOptions option = new CookieOptions();
            option.HttpOnly = true;
            if (user.encryptedJWT == "")
            {
                option.Expires = DateTime.Now.AddDays(-1);
            }
            option.MaxAge = TimeSpan.FromHours(1);

            option.SameSite = SameSiteMode.None;
            option.Secure = true;
            _httpContextAccessor.HttpContext.Response.Cookies.Append("jwt", user.encryptedJWT, option);

            return Ok();
        }


        [HttpGet()]
        public IActionResult GetCookie()
        {
            string jwt = _httpContextAccessor.HttpContext.Request.Cookies["jwt"];
            if (jwt == "" || jwt == null)
            {
                return Ok("");
            } else
            {
                return Ok(jwt);
            }
   
        }
    }
}