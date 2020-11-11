using System;
using System.Collections.Generic;
using System.Text;

using MyBatiment.Core.Models;
using MyBatiment.Core.Repositories;

namespace MyBatiment.Data.Repositories
{
    class ProjectItemRepository : Repository<ProjectItem>, IProjectItemRepository
    {
        private MyDbContext MyDbContext
        {
            get { return Context as MyDbContext; }
        }
        public ProjectItemRepository(MyDbContext context)
            : base(context)
        { }
    }
}
