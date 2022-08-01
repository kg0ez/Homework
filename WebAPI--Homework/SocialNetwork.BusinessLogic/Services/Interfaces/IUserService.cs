using SocialNetwork.Model.DatabaseModels;
using SocialNetwork.Model.DTOs;

namespace SocialNetwork.BusinessLogic.Services.Interfaces
{
	public interface IUserService
	{
		public bool Register(SignInOrUpDto registerDto, RefreshTokenDto tokenDto);
		UserDto Login(SignInOrUpDto loginDto);
		void Update(User user, RefreshTokenDto tokenDto);
		User Get(string name);
		User Get(int id);
		IEnumerable<User> Get();
		bool Delete(int id);
	}
}

