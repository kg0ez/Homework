using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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
        public UserController(IUserService userService)
        {
            _userService = userService;
        }
        //[HttpPost("register")]
        //public ActionResult<UserDto> Register(RegisterDto userDto)
        //{
        //    if (_userService.UserExists(userDto.Login))
        //        return BadRequest("UserName Is Already Taken");
        //    var user = _userService.Register(userDto);
        //    if (user == null) return BadRequest();
        //    return Ok(user);
        //}
        [HttpPost("register")]
        public IActionResult Register(SignInOrUpDto dto)
        {
            if (_userService.UserExists(dto.Login))
                return BadRequest("User alredy Exists");
            return _userService.Register(dto) ? Ok("User registered") : BadRequest("Failed to register");
        }

        [HttpPost("login")]
        public ActionResult<UserDto> Login(SignInOrUpDto loginDto)
        {
            var user = _userService.Login(loginDto);
            if (user == null)
                return Unauthorized("Invalid Login or Password");
            return Ok(user);
        }
        [HttpGet("Get"), Authorize]
        public ActionResult<IEnumerable<string>> Get()
        {
            return new string[] { "value1", "value2" };
        }
    }
}

