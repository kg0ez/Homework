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
        private readonly ITokenService _token;
        private readonly ApplicationContext _db;
        private readonly IMapper _mapper;

        public UserService(ApplicationContext context,
            IMapper mapper,
            ITokenService tokenService)
        {
            _db = context;
            _mapper = mapper;
            _token = tokenService;
        }

        public bool Register(SignInOrUpDto dto, RefreshTokenDto tokenDto)
        {
            var user = _mapper.Map<User>(dto);
            user = _token.UpdateRefreshToken(user, tokenDto);
            _db.Users.Add(user);
            return Save();
        }

        public void Update(User user, RefreshTokenDto tokenDto)
        {
            _db.Users.Update(_token.UpdateRefreshToken(user,tokenDto));
            _db.SaveChanges();
        }

        public bool Save()=>
            _db.SaveChanges() > 0 ? true : false;
        
        public UserDto Login(SignInOrUpDto loginDto)
        {
            var user = _db.Users
                .SingleOrDefault(x => x.Login == loginDto.Login);

            if (user == null) return null!;

            var hmac = new HMACSHA512(user.PasswordSalt);
            var computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(loginDto.Password));

            for (int i = 0; i < computedHash.Length; i++)
                if (computedHash[i] != user.PasswordHash[i]) return null!;

            var userDto = _mapper.Map<User, UserDto>(user);
            userDto.Token = _token.CreateToken(user);

            return userDto;
        }
        
        public bool Delete(int id)
        {
            var hero = _db.Users.FirstOrDefault(x => x.Id == id);
            if (hero == null)
                return false;
            _db.Users.Remove(hero);
            return Save();
        }

        public User GetByName(string login)=>
            _db.Users.FirstOrDefault(u => u.Login == login.ToLower())!;

        public User GetById(int id)=>
            _db.Users.FirstOrDefault(u => u.Id ==id)!;

        public IEnumerable<User> GetUsers() =>
            _db.Users.AsNoTracking().ToList();
    }
}

