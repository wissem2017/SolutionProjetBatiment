using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using AutoMapper;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

using MyBatiment.API.Resources;
using MyBatiment.API.Validation;
using MyBatiment.Core.Models;
using MyBatiment.Core.Services;

namespace MyBatiment.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProjectItemController : ControllerBase
    {
        private readonly IProjectItemService _serviceProjectItem;
        private readonly IMapper _mapperService;

        public ProjectItemController(IProjectItemService serviceProjectItem, IMapper mapperService)
        {
            _serviceProjectItem = serviceProjectItem;
            _mapperService = mapperService;

        }

        [HttpGet("")]
        public async Task<ActionResult<IEnumerable<ProjectItemResource>>> GETAllProjectItems()
        {
            try
            {
                var projectItems = await _serviceProjectItem.GetAllProjectItems();
                //--> Mpper permet de convertir la liste des ProjectItem vers ProjectItemResource
                var projectItemResources = _mapperService.Map<IEnumerable<ProjectItem>, IEnumerable<ProjectItemResource>>(projectItems);
                return Ok(projectItemResources);
            }
            catch (Exception ex)
            {

                return BadRequest(ex.Message);
            }
        }

        [HttpGet("{id}")]
       
        public async Task<ActionResult<ProjectItemResource>> GetProjectItem(Guid id)
        {
            try
            {
                var projectItem = await _serviceProjectItem.GetProjectItemById(id);

                if (projectItem == null)
                    return NotFound();

                //--> Mpper permet de convertir la liste des ProjectItem vers ProjectItemResource
                var projectItemResource = _mapperService.Map<ProjectItem, ProjectItemResource>(projectItem);

                return Ok(projectItemResource);
            }
            catch (Exception ex)
            {

                return BadRequest(ex.Message);
            }
        }

        [HttpPost("")]
        [Authorize]
        public async Task<ActionResult<ProjectItemResource>> CreateProjectItem(SaveProjectItemResource saveProjectItemResource)
        {
            //GET Current user
            var userId = User.Identity.Name;

            // Validation
            var validation = new SaveProjectItemResourceValidator();
            var validationResult = await validation.ValidateAsync(saveProjectItemResource);

            if (!validationResult.IsValid)
                return BadRequest(validationResult.Errors);

            // mappage de SaveProjectItemResource vers ProjectItem
            var projectItem = _mapperService.Map<SaveProjectItemResource, ProjectItem>(saveProjectItemResource);

            // Creation newProjectItem
            var newProjectItem = await _serviceProjectItem.CreateProjectItem(projectItem);

            // mappage de ProjectItem vers ProjectItemResource
            var projectItemResource = _mapperService.Map<ProjectItem, ProjectItemResource>(newProjectItem);
            return Ok(projectItemResource);
        }

        [HttpPut("{id}")]
        [Authorize]
        public async Task<ActionResult<ProjectItemResource>> UpdateProjectItem(Guid id, SaveProjectItemResource saveProjectItemResource)
        {
            /// validation
            var validation = new SaveProjectItemResourceValidator();
            var validationResult = await validation.ValidateAsync(saveProjectItemResource);

            if (!validationResult.IsValid) return BadRequest(validationResult.Errors);

            // si la ProjectItem existe depuis le id
            var projectItemUpdate = await _serviceProjectItem.GetProjectItemById(id);
            if (projectItemUpdate == null) return BadRequest("Project n'existe pas ");

            var projectItem = _mapperService.Map<SaveProjectItemResource, ProjectItem>(saveProjectItemResource);

            await _serviceProjectItem.UpdateProjectItem(projectItemUpdate,projectItem);
            var projectItemUpdateNew = await _serviceProjectItem.GetProjectItemById(id);
            var projectItemResourceUpdate = _mapperService.Map<ProjectItem, SaveProjectItemResource>(projectItemUpdateNew);

            return Ok(projectItemResourceUpdate);
        }

        [HttpDelete("{id}")]
        [Authorize]
        public async Task<ActionResult> DeleteProjectItem(Guid id)
        {
            var projectItem = await _serviceProjectItem.GetProjectItemById(id);
            if (projectItem == null) return BadRequest("Project n'existe pas");

            await _serviceProjectItem.DeleteProjectItem(projectItem);
            return NoContent();
        }
    }
}
