namespace SocialNetwork.BusinessLogic.Services.Interfaces
{
	public interface IVerificationService
	{
		void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt);
		bool VerifyPasswordHash(string password, byte[] passwordHash, byte[] passwordSalt);
		bool UserExists(string login);
	}
}

