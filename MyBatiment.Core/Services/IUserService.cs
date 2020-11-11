using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

using MyBatiment.Core.Models;

namespace MyBatiment.Core.Services
{
    public interface IUserService
    {
        Task<User> Authenticate(string username, string password);
        Task<IEnumerable<User>> GetAll();
        Task<User> GetById(Guid id);
        Task<User> Create(User user, string password);
        void Update(User user, string password = null);
        void Delete(Guid id);
    }
}
