using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

using MyBatiment.Core.Repositories;

namespace MyBatiment.Core
{
    public interface IUnitOfWork : IDisposable
    {
        IOwnerRepository Owners { get; }
        IProjectItemRepository ProjectItems { get; }
        IServiceItemRepository ServiceItems { get; }
        IUserRepository Users { get; }

        Task<int> CommitAsync();
    }
}
