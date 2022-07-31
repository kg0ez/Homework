using System;
using SocialNetwork.Model.DTOs;

namespace SocialNetwork.BusinessLogic.Services.Interfaces
{
	public interface IUserService
	{
		public bool Register(SignInOrUpDto registerDto);
		//public UserDto Register(RegisterDto registerDto);
		UserDto Login(SignInOrUpDto loginDto);
		bool UserExists(string login);
		bool Save();
	}
}

