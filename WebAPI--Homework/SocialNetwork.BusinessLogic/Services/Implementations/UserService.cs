using System;
using System.Security.Cryptography;
using System.Text;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using SocialNetwork.BusinessLogic.Services.Interfaces;
using SocialNetwork.Model.Database;
using SocialNetwork.Model.DatabaseModels;
using SocialNetwork.Model.DTOs;

namespace SocialNetwork.BusinessLogic.Services.Implementations
{
	public class UserService: IUserService
	{
        private readonly ITokenService _tokenService;
        private readonly ApplicationContext _context;
        private readonly IMapper _mapper;
        public UserService(ApplicationContext context,
            IMapper mapper,
            ITokenService tokenService)
        {
            _context = context;
            _mapper = mapper;
            _tokenService = tokenService;
        }
        public bool Register(SignInOrUpDto dto)
        {
            var user = _mapper.Map<User>(dto);
            _context.Users.Add(user);
            return Save();
        }

        public bool Save()
        {
            return _context.SaveChanges() > 0 ? true : false;
        }
        //public UserDto Register(RegisterDto registerDto)
        //{
        //    try
        //    {
        //        var hmac = new HMACSHA512();
        //        var user = _mapper.Map<RegisterDto, User>(registerDto);
        //        user.PasswordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(registerDto.Password));
        //        user.PasswordSalt = hmac.Key;
        //        _context.Users.Add(user);
        //        _context.SaveChanges();

        //        var userDto = _mapper.Map<User, UserDto>(user);
        //        userDto.Token = _tokenService.CreateToken(user);
        //        return userDto;
        //    }
        //    catch (Exception)
        //    {
        //        return null!;
        //    }

        //}
        public UserDto Login(SignInOrUpDto loginDto)
        {
            var user = _context.Users
                .SingleOrDefault(x => x.Login == loginDto.Login);

            if (user == null) return null!;

            var hmac = new HMACSHA512(user.PasswordSalt);
            var computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(loginDto.Password));
            for (int i = 0; i < computedHash.Length; i++)
            {
                if (computedHash[i] != user.PasswordHash[i]) return null!;
            }
            var userDto = _mapper.Map<User, UserDto>(user);
            userDto.Token = _tokenService.CreateToken(user);

            return userDto;
        }
        public bool UserExists(string login)
        {
            return _context.Users
                .AsNoTracking()
                .Any(x => x.Login == login.ToLower()) ? true : false;
        }
    }
}

