using System.Security.Cryptography;
using System.Text;
using Microsoft.EntityFrameworkCore;
using SocialNetwork.BusinessLogic.Services.Interfaces;
using SocialNetwork.Model.Database;

namespace SocialNetwork.BusinessLogic.Services.Implementations
{
	public class VerificationService: IVerificationService
	{
        private readonly ApplicationContext _db;

		public VerificationService(ApplicationContext context)=>
            _db = context;

        public void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
        {
            using (var hmac = new HMACSHA256())
            {
                passwordSalt = hmac.Key;
                passwordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
            }
        }
        public bool IsVerifyPasswordHash(string password, byte[] passwordHash, byte[] passwordSalt)
        {
            using (var hmac = new HMACSHA256(passwordSalt))
            {
                var computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
                return computedHash.SequenceEqual(passwordHash);
            }
        }
        public bool IsUserExists(string login)
        {
            return _db.Users
                .AsNoTracking()
                .Any(x => x.Login == login.ToLower()) ? true : false;
        }

    }
}

