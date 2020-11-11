using System;
using System.Collections.Generic;
using System.Text;

using MyBatiment.Core.Models;
using MyBatiment.Core.Repositories;

namespace MyBatiment.Data.Repositories
{
    public class OwnerRepository : Repository<Owner>, IOwnerRepository
    {
        private MyDbContext MyDbContext
        {
            get { return Context as MyDbContext; }
        }
        public OwnerRepository(MyDbContext context)
            : base(context)
        { }
    }
}
