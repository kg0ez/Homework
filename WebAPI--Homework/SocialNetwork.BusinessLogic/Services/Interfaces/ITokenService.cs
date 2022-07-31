using System;
using SocialNetwork.Model.DatabaseModels;

namespace SocialNetwork.BusinessLogic.Services.Interfaces
{
	public interface ITokenService
	{
		string CreateToken(User user);
	}
}

