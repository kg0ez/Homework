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

        public bool Register(SignInOrUpDto dto, RefreshTokenDto tokenDto)
        {
            var user = _mapper.Map<User>(dto);
            user = UpdateUser(user, tokenDto);
            _context.Users.Add(user);
            return Save();
        }

        public User UpdateUser(User user, RefreshTokenDto tokenDto)
        {
            user.TokenCreated = tokenDto.Created;
            user.TokenExpires = tokenDto.Expires;
            user.RefreshToken = tokenDto.Token;
            return user;
        }

        public void UpdateDB(User user, RefreshTokenDto tokenDto)
        {
            _context.Users.Update(UpdateUser(user,tokenDto));
            _context.SaveChanges();
        }

        public bool Save()=>
            _context.SaveChanges() > 0 ? true : false;
        
        public UserDto Login(SignInOrUpDto loginDto)
        {
            var user = _context.Users
                .SingleOrDefault(x => x.Login == loginDto.Login);

            if (user == null) return null!;

            var hmac = new HMACSHA512(user.PasswordSalt);
            var computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(loginDto.Password));

            for (int i = 0; i < computedHash.Length; i++)
                if (computedHash[i] != user.PasswordHash[i]) return null!;

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

        public void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
        {
            using (var hmac = new HMACSHA256())
            {
                passwordSalt = hmac.Key;
                passwordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
            }
        }
        public bool VerifyPasswordHash(string password, byte[] passwordHash, byte[] passwordSalt)
        {
            using (var hmac = new HMACSHA256(passwordSalt))
            {
                var computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
                return computedHash.SequenceEqual(passwordHash);
            }
        }

        public User GetByName(string login)=>
            _context.Users.FirstOrDefault(u => u.Login == login.ToLower())!;

        public User GetById(int id)=>
            _context.Users.FirstOrDefault(u => u.Id ==id)!;

        public IEnumerable<User> GetUsers() =>
            _context.Users.AsNoTracking().ToList();
    }
}

