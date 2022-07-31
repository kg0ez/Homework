using SocialNetwork.Model.DatabaseModels;
using SocialNetwork.Model.DTOs;

namespace SocialNetwork.BusinessLogic.Services.Interfaces
{
	public interface IUserService
	{
		public bool Register(SignInOrUpDto registerDto, RefreshTokenDto tokenDto);
		UserDto Login(SignInOrUpDto loginDto);
		bool UserExists(string login);
		bool Save();
		void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt);
		bool VerifyPasswordHash(string password, byte[] passwordHash, byte[] passwordSalt);
		User UpdateUser(User user, RefreshTokenDto tokenDto);
		void UpdateDB(User user, RefreshTokenDto tokenDto);
		User GetByName(string login);
		User GetById(int id);
		IEnumerable<User> GetUsers();
	}
}

