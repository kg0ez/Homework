using System;
using SocialNetwork.Model.DTOs;

namespace SocialNetwork.BusinessLogic.Services.Interfaces
{
	public interface IUserService
	{
		public bool Register(SignInOrUpDto registerDto);
		//public UserDto Register(RegisterDto registerDto);
		public UserDto Login(SignInOrUpDto loginDto);
		public bool UserExists(string login);
	}
}

