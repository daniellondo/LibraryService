namespace Api.Controllers
{
    using Api.Utils;
    using Domain.Dtos;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Configuration;

    [Route("api/[controller]")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        private readonly IConfiguration _config;
        public LoginController(IConfiguration config)
        {
            _config = config;
        }

        /// <summary>
        ///     Username: admin - Password: admin
        /// </summary>
        /// <param name="userLogin"></param>
        [AllowAnonymous]
        [HttpPost]
        public IActionResult Login([FromBody] UserLogin userLogin)
        {
            var user = LoginUtils.Authenticate(userLogin);
            if (user != null)
            {
                var token = LoginUtils.GenerateToken(user, _config);
                return Ok(token);
            }

            return NotFound("user not found");
        }

    }
}
