using Microsoft.AspNetCore.Mvc;
using SocialNetwork.BusinessLogic.Services.Interfaces;
using SocialNetwork.Model.DTOs;

namespace SocialNetwork.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : Controller
    {
        private readonly IUserService _userService;
        private readonly ITokenService _tokenService;
        private readonly IVerificationService _verification;

        public AuthController(IUserService userService,
            ITokenService tokenService,
            IVerificationService verification)
        {
            _userService = userService;
            _tokenService = tokenService;
            _verification = verification;
        }
        [HttpPost("register")]
        public IActionResult Register(SignInOrUpDto dto)
        {
            if (_verification.IsUserExists(dto.Login))
                return BadRequest("User alredy Exists");
            var newRefreshToken = _tokenService.GenerateRefreshToken();
            SetRefreshToken(newRefreshToken);
            return _userService.Register(dto, newRefreshToken) ? Ok("User was registered") : BadRequest("Failed to register");
        }

        [HttpPost("login")]
        public IActionResult Login(SignInOrUpDto request)
        {
            var user = _userService.Get(request.Login);
            if (user == null)
                return BadRequest("User not found.");

            if (!_verification.IsVerifyPasswordHash(request.Password, user.PasswordHash, user.PasswordSalt))
                return BadRequest("Wrong password.");

            string token = _tokenService.CreateToken(user);

            var refreshToken = _tokenService.GenerateRefreshToken();

            SetRefreshToken(refreshToken);
            _userService.Update(user, refreshToken);

            return Ok(token);
        }
        private void SetRefreshToken(RefreshTokenDto newRefreshToken)
        {
            var cookieOptions = new CookieOptions
            {
                HttpOnly = true,
                Expires = newRefreshToken.Expires
            };
            Response.Cookies.Append("refreshToken", newRefreshToken.Token, cookieOptions);
        }
    }
}

