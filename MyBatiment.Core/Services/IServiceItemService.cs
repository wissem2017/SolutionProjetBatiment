using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

using MyBatiment.Core.Models;

namespace MyBatiment.Core.Services
{
    public interface IServiceItemService
    {
        Task<IEnumerable<ServiceItem>> GetAllServiceItems();
        Task<ServiceItem> GetServiceItemById(Guid id);
        Task<ServiceItem> CreateServiceItem(ServiceItem newServiceItem);
        Task UpdateServiceItem(ServiceItem serviceItemToBeUpdated, ServiceItem serviceItem);
        Task DeleteServiceItem(ServiceItem serviceItem);
    }
}
