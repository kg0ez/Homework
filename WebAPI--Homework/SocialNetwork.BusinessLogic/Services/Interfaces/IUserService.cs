using SocialNetwork.Model.DatabaseModels;
using SocialNetwork.Model.DTOs;

namespace SocialNetwork.BusinessLogic.Services.Interfaces
{
	public interface IUserService
	{
		public bool Register(SignInOrUpDto registerDto, RefreshTokenDto tokenDto);
		UserDto Login(SignInOrUpDto loginDto);
		bool Save();
		void Update(User user, RefreshTokenDto tokenDto);
		User GetByName(string login);
		User GetById(int id);
		IEnumerable<User> GetUsers();
		bool Delete(int id);
	}
}

