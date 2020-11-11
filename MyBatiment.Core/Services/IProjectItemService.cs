using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

using MyBatiment.Core.Models;

namespace MyBatiment.Core.Services
{
    public interface IProjectItemService
    {
        Task<IEnumerable<ProjectItem>> GetAllProjectItems();
        Task<ProjectItem> GetProjectItemById(Guid id);
        Task<ProjectItem> CreateProjectItem(ProjectItem newProjectItem);
        Task UpdateProjectItem(ProjectItem projectItemToBeUpdated, ProjectItem projectItem);
        Task DeleteProjectItem(ProjectItem projectItem);
    }
}
