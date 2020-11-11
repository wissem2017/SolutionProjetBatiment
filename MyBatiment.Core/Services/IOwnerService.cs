using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

using MyBatiment.Core.Models;

namespace MyBatiment.Core.Services
{
    public interface IOwnerService
    {
        Task<IEnumerable<Owner>> GetAllOwners();
        Task<Owner> GetOwnerById(Guid id);
        Task<Owner> CreateOwner(Owner newOwner);
        Task UpdateOwner(Owner ownerToBeUpdated, Owner owner);
        Task DeleteOwner(Owner owner);
    }
}
