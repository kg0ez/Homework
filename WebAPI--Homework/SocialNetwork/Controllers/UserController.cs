using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SocialNetwork.BusinessLogic.Services.Interfaces;

namespace SocialNetwork.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)=>
            _userService = userService;
        
        [HttpDelete("DeleteAccount"),Authorize]
        public IActionResult DeleteAccount([FromQuery] int id) =>
            _userService.Delete(id) ? Ok("User was deleted") : BadRequest("User wasnt deleted");

        [HttpGet("GetUsers"), Authorize]
        public IActionResult Get()=>
            Ok(_userService.Get());
    }
}

