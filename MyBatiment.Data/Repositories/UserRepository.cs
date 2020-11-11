using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.EntityFrameworkCore;

using MyBatiment.Core.Models;
using MyBatiment.Core.Repositories;

namespace MyBatiment.Data.Repositories
{
    public class UserRepository : Repository<User>, IUserRepository
    {
        public UserRepository(MyDbContext context)
        : base(context)
        { }
        private MyDbContext MyDbContext
        {
            get { return Context as MyDbContext; }
        }

        public async Task<User> Authenticate(string username, string password)
        {
            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
                return null;

            var user = await MyDbContext.Users.SingleOrDefaultAsync(x => x.Username == username);

            // check if username exists
            if (user == null)
                return null;

            // check if password is correct
            if (!VerifyPasswordHash(password, user.PasswordHash, user.PasswordSalt))
                return null;

            // authentication successful
            return user;
        }
        public async Task<User> Create(User user, string password)
        {
            // validation
            if (string.IsNullOrWhiteSpace(password))
                throw new Exception("Mot de passe requis");
            var resultUser = await MyDbContext.Users.AnyAsync(x => x.Username == user.Username);
            if (resultUser)
                throw new Exception("Nom d'utilisateur \"" + user.Username + "\" est déjà pris");

            byte[] passwordHash, passwordSalt;
            CreatePasswordHash(password, out passwordHash, out passwordSalt);

            user.PasswordHash = passwordHash;
            user.PasswordSalt = passwordSalt;

            await MyDbContext.Users.AddAsync(user);

            return user;
        }

        public void Update(User userParam, string password = null)
        {
            var user = MyDbContext.Users.Find(userParam.Id);

            if (user == null)
                throw new Exception("User not found");

            if (userParam.Username != user.Username)
            {
                // username has changed so check if the new username is already taken
                if (MyDbContext.Users.Any(x => x.Username == userParam.Username))
                    throw new Exception("Nom d'utilisateur " + userParam.Username + " est déjà pris");
            }

            // update user properties
            user.FirstName = userParam.FirstName;
            user.LastName = userParam.LastName;
            user.Username = userParam.Username;

            // update password if it was entered
            if (!string.IsNullOrWhiteSpace(password))
            {
                byte[] passwordHash, passwordSalt;
                CreatePasswordHash(password, out passwordHash, out passwordSalt);

                user.PasswordHash = passwordHash;
                user.PasswordSalt = passwordSalt;
            }

            MyDbContext.Users.Update(user);
        }

        public async Task<IEnumerable<User>> GetAllUserAsync()
        {
            return await MyDbContext.Users
             .ToListAsync();
        }
        public async Task<User> GetWithUsersByIdAsync(Guid id)
        {
            return await MyDbContext.Users
                     .Where(user => user.Id == id)
                     .FirstOrDefaultAsync();
        }

        private static void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
        {
            if (password == null) throw new ArgumentNullException("password");
            if (string.IsNullOrWhiteSpace(password)) throw new ArgumentException("Value cannot be empty or whitespace only string.", "password");

            using (var hmac = new System.Security.Cryptography.HMACSHA512())
            {
                passwordSalt = hmac.Key;
                passwordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
            }
        }
        private static bool VerifyPasswordHash(string password, byte[] storedHash, byte[] storedSalt)
        {
            if (password == null) throw new ArgumentNullException("password");
            if (string.IsNullOrWhiteSpace(password)) throw new ArgumentException("Value cannot be empty or whitespace only string.", "password");
            if (storedHash.Length != 64) throw new ArgumentException("Invalid length of password hash (64 bytes expected).", "passwordHash");
            if (storedSalt.Length != 128) throw new ArgumentException("Invalid length of password salt (128 bytes expected).", "passwordHash");

            using (var hmac = new System.Security.Cryptography.HMACSHA512(storedSalt))
            {
                var computedHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
                for (int i = 0; i < computedHash.Length; i++)
                {
                    if (computedHash[i] != storedHash[i]) return false;
                }
            }

            return true;
        }

        public void Delete(Guid id)
        {
            var user = MyDbContext.Users.Find(id);
            if (user != null)
            {
                MyDbContext.Users.Remove(user);
            }
        }
    }
}
