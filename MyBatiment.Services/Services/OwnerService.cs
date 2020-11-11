using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

using MyBatiment.Core;
using MyBatiment.Core.Models;
using MyBatiment.Core.Services;

namespace MyBatiment.Services.Services
{
    public class OwnerService : IOwnerService
    {
        private readonly IUnitOfWork _unitOfWork;

        public OwnerService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<Owner> CreateOwner(Owner newOwner)
        {
            await _unitOfWork.Owners.AddAsync(newOwner);
            await _unitOfWork.CommitAsync();
            return newOwner;
        }

        public async Task DeleteOwner(Owner owner)
        {
            _unitOfWork.Owners.Remove(owner);
            await _unitOfWork.CommitAsync();
        }

        public async Task<IEnumerable<Owner>> GetAllOwners()
        {
            return await _unitOfWork.Owners.GetAllAsync();
        }

        public async Task<Owner> GetOwnerById(Guid id)
        {
            return await _unitOfWork.Owners
                .GetByIdAsync(id);
        }

        public async Task UpdateOwner(Owner ownerToBeUpdated, Owner owner)
        {
            ownerToBeUpdated.Title = owner.Title;
            ownerToBeUpdated.Description = owner.Description;
            ownerToBeUpdated.Adress = owner.Adress;
            ownerToBeUpdated.Tel = owner.Tel;
            ownerToBeUpdated.Image = owner.Image;


            await _unitOfWork.CommitAsync();
        }
    }
}
