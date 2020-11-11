using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

using MyBatiment.Core;
using MyBatiment.Core.Repositories;
using MyBatiment.Data.Repositories;

namespace MyBatiment.Data
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly MyDbContext _context;
        private IOwnerRepository _ownerRepository;
        private IProjectItemRepository _projectItemRepository;
        private IServiceItemRepository _serviceItemRepository;
        private IUserRepository _userRepository;


        public UnitOfWork(MyDbContext context)
        {
            this._context = context;
        }
        public IOwnerRepository Owners => _ownerRepository = _ownerRepository ?? new OwnerRepository(_context);

        public IProjectItemRepository ProjectItems => _projectItemRepository = _projectItemRepository ?? new ProjectItemRepository(_context);

        public IServiceItemRepository ServiceItems => _serviceItemRepository = _serviceItemRepository ?? new ServiceItemRepository(_context);

        public IUserRepository Users => _userRepository = _userRepository ?? new UserRepository(_context);

        public async Task<int> CommitAsync()
        {
            return await _context.SaveChangesAsync();
        }
        public void Dispose()
        {
            _context.Dispose();
        }
    }
}
