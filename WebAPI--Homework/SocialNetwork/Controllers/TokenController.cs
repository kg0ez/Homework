using Microsoft.AspNetCore.Mvc;
using SocialNetwork.BusinessLogic.Services.Interfaces;

namespace SocialNetwork.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TokenController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly ITokenService _tokenService;

        public TokenController(IUserService userService,
            ITokenService tokenService)
        {
            _userService = userService;
            _tokenService = tokenService;
        }

        [HttpPost("refresh-token")]
        public ActionResult<string> RefreshToken([FromQuery] int id)
        {
            var user = _userService.Get(id);
            if (user == null)
                return BadRequest("User is not found");

            var refreshToken = Request.Cookies["refreshToken"];

            if (!user.RefreshToken.Equals(refreshToken))
                return Unauthorized("Invalid Refresh Token.");
            else if (user.TokenExpires < DateTime.Now)
                return Unauthorized("Token expired.");

            string token = _tokenService.CreateToken(user);

            var newRefreshToken = _tokenService.GenerateRefreshToken();

            _userService.Update(user, newRefreshToken);

            return Ok(token);
        }
    }
}

