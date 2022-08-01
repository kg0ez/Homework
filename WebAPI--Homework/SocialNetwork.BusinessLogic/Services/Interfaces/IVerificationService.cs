namespace SocialNetwork.BusinessLogic.Services.Interfaces
{
	public interface IVerificationService
	{
		void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt);
		bool IsVerifyPasswordHash(string password, byte[] passwordHash, byte[] passwordSalt);
		bool IsUserExists(string login);
	}
}

