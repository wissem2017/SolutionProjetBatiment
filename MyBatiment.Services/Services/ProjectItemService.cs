using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

using MyBatiment.Core;
using MyBatiment.Core.Models;
using MyBatiment.Core.Services;

namespace MyBatiment.Services.Services
{
    public class ProjectItemService : IProjectItemService
    {
        private readonly IUnitOfWork _unitOfWork;

        public ProjectItemService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<ProjectItem> CreateProjectItem(ProjectItem newProjectItem)
        {
            await _unitOfWork.ProjectItems.AddAsync(newProjectItem);
            await _unitOfWork.CommitAsync();
            return newProjectItem;
        }

        public async Task DeleteProjectItem(ProjectItem ProjectItem)
        {
            _unitOfWork.ProjectItems.Remove(ProjectItem);
            await _unitOfWork.CommitAsync();
        }

        public async Task<IEnumerable<ProjectItem>> GetAllProjectItems()
        {
            return await _unitOfWork.ProjectItems.GetAllAsync();
        }

        public async Task<ProjectItem> GetProjectItemById(Guid id)
        {
            return await _unitOfWork.ProjectItems.GetByIdAsync(id);
        }

        public async Task UpdateProjectItem(ProjectItem projectItemToBeUpdated, ProjectItem projectItem)
        {
            projectItemToBeUpdated.ProjectName = projectItem.ProjectName;
            projectItemToBeUpdated.Description = projectItem.Description;
            projectItemToBeUpdated.Image = projectItem.Image;

            await _unitOfWork.CommitAsync();
        }
    }
}
