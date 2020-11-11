using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

using MyBatiment.Core.Models;

namespace MyBatiment.Core.Repositories
{
    public interface IUserRepository
    {
        Task<User> Authenticate(string username, string password);
        Task<User> Create(User user, string password);
        void Update(User user, string password = null);
        void Delete(Guid id);
        Task<IEnumerable<User>> GetAllUserAsync();
        Task<User> GetWithUsersByIdAsync(Guid id);
    }
}
