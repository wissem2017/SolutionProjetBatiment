using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

using MyBatiment.Core.Models;
using MyBatiment.Core.Repositories;

namespace MyBatiment.Data.Repositories
{
    class ServiceItemRepository : Repository<ServiceItem>, IServiceItemRepository
    {
        private MyDbContext MyDbContext
        {
            get { return Context as MyDbContext; }
        }
        public ServiceItemRepository(MyDbContext context)
            : base(context)
        { }
    }
}
