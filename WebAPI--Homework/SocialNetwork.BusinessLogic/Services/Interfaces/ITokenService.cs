using SocialNetwork.Model.DatabaseModels;
using SocialNetwork.Model.DTOs;

namespace SocialNetwork.BusinessLogic.Services.Interfaces
{
	public interface ITokenService
	{
		string CreateToken(User user);
		RefreshTokenDto GenerateRefreshToken();
	}
}

