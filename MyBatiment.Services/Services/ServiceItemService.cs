using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

using MyBatiment.Core;
using MyBatiment.Core.Models;
using MyBatiment.Core.Services;

namespace MyBatiment.Services.Services
{
    public class ServiceItemService : IServiceItemService
    {
        private readonly IUnitOfWork _unitOfWork;

        public ServiceItemService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<ServiceItem> CreateServiceItem(ServiceItem newServiceItem)
        {
            await _unitOfWork.ServiceItems.AddAsync(newServiceItem);
            await _unitOfWork.CommitAsync();
            return newServiceItem;
        }

        public async Task DeleteServiceItem(ServiceItem serviceItem)
        {
            _unitOfWork.ServiceItems.Remove(serviceItem);
            await _unitOfWork.CommitAsync();
        }

        public async Task<IEnumerable<ServiceItem>> GetAllServiceItems()
        {
            return await _unitOfWork.ServiceItems.GetAllAsync();
        }

        public async Task<ServiceItem> GetServiceItemById(Guid id)
        {
            return await _unitOfWork.ServiceItems.GetByIdAsync(id);
        }

     

        public async Task UpdateServiceItem(ServiceItem serviceItemToBeUpdated, ServiceItem serviceItem)
        {
            serviceItemToBeUpdated.ServiceName = serviceItem.ServiceName;
            serviceItemToBeUpdated.Description = serviceItem.Description;
            serviceItemToBeUpdated.Image = serviceItem.Image;
          
            await _unitOfWork.CommitAsync();
        }
    }
}
