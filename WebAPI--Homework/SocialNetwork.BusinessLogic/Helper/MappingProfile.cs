using System.Text;
using AutoMapper;
using SocialNetwork.Model.DatabaseModels;
using SocialNetwork.Model.DTOs;
using System.Security.Cryptography;

namespace SocialNetwork.BusinessLogic.Helper
{
	public class MappingProfile:Profile
	{
		public MappingProfile()
		{
			var hmac = new HMACSHA256();

			CreateMap<User, SignInOrUpDto>();
			CreateMap<SignInOrUpDto, User>()
				.ForMember("Login", opt => opt.MapFrom(ud => ud.Login.ToLower()))
				.ForMember("PasswordHash", opt => opt.MapFrom(ud => hmac.ComputeHash(Encoding.UTF8.GetBytes(ud.Password))))
				.ForMember("PasswordSalt", opt => opt.MapFrom(ud => hmac.Key));
			CreateMap<User, UserDto>();
			CreateMap<UserDto, User>();
			CreateMap<RefreshTokenDto, User>()
				.ForMember("RefreshToken", opt => opt.MapFrom(opt => opt.Token))
				.ForMember("TokenExpires", opt => opt.MapFrom(opt => opt.Expires))
				.ForMember("TokenCreated", opt => opt.MapFrom(opt => opt.Created));
		}
	}
}

