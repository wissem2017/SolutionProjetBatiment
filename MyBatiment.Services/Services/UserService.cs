using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

using MyBatiment.Core;
using MyBatiment.Core.Models;
using MyBatiment.Core.Services;

namespace MyBatiment.Services.Services
{
    public class UserService : IUserService
    {
        private readonly IUnitOfWork _unitOfWork;
        public UserService(IUnitOfWork unitOfWork)
        {
            this._unitOfWork = unitOfWork;
        }

        public async Task<User> Authenticate(string username, string password)
        {
            return await _unitOfWork.Users.Authenticate(username, password);
        }

        public async Task<User> Create(User user, string password)
        {
            await _unitOfWork.Users.Create(user, password);

            await _unitOfWork.CommitAsync();
            return user;
        }

        public void Delete(Guid id)
        {
            _unitOfWork.Users.Delete(id);
            _unitOfWork.CommitAsync();
        }

        public async Task<IEnumerable<User>> GetAll()
        {
            return await _unitOfWork.Users.GetAllUserAsync();
        }

        public async Task<User> GetById(Guid id)
        {
            return await _unitOfWork.Users.GetWithUsersByIdAsync(id);
        }

        public void Update(User user, string password = null)
        {
            _unitOfWork.Users.Update(user, password);
            _unitOfWork.CommitAsync();
        }

    }
}
