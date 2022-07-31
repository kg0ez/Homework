using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SocialNetwork.BusinessLogic.Services.Interfaces;
using SocialNetwork.Model.DTOs;

namespace SocialNetwork.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly ITokenService _tokenService;

        public UserController(IUserService userService, ITokenService tokenService)
        {
            _userService = userService;
            _tokenService = tokenService;
        }
        
        [HttpPost("register")]
        public IActionResult Register(SignInOrUpDto dto)
        {
            if (_userService.UserExists(dto.Login))
                return BadRequest("User alredy Exists");
            var newRefreshToken = _tokenService.GenerateRefreshToken();
            SetRefreshToken(newRefreshToken);
            return _userService.Register(dto, newRefreshToken) ? Ok("User registered") : BadRequest("Failed to register");
        }

        [HttpPost("login")]
        public IActionResult Login(SignInOrUpDto request)
        {
            var user = _userService.GetByName(request.Login);
            if (user == null)
                return BadRequest("User not found.");

            if (!_userService.VerifyPasswordHash(request.Password, user.PasswordHash, user.PasswordSalt))
                return BadRequest("Wrong password.");

            string token = _tokenService.CreateToken(user);

            var refreshToken = _tokenService.GenerateRefreshToken();

            SetRefreshToken(refreshToken);
            _userService.UpdateDB(user, refreshToken);

            return Ok(token);
        }

        [HttpGet("GetUsers"), Authorize]
        public IActionResult Get()=>
            Ok(_userService.GetUsers());

        [HttpPost("refresh-token")]
        public ActionResult<string> RefreshToken([FromQuery] int id)
        {
            var user = _userService.GetById(id);
            if (user == null)
                return BadRequest("User is not found");

            var refreshToken = Request.Cookies["refreshToken"];

            if (!user.RefreshToken.Equals(refreshToken))
                return Unauthorized("Invalid Refresh Token.");
            else if (user.TokenExpires < DateTime.Now)
                return Unauthorized("Token expired.");

            string token = _tokenService.CreateToken(user);

            var newRefreshToken = _tokenService.GenerateRefreshToken();
            
            _userService.UpdateDB(user, newRefreshToken);

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

