using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using System.Threading.Tasks;

using AutoMapper;

using Microsoft.AspNetCore.Authorization;

//using AutoMapper;

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
    public class OwnerController : ControllerBase
    {
        private readonly IOwnerService _serviceOwner;
       private readonly IMapper _mapperService;

        public OwnerController(IOwnerService serviceOwner, IMapper mapperService)
        {
            _serviceOwner = serviceOwner;
            _mapperService = mapperService;

        }

        [HttpGet("")]
        
        public async Task<ActionResult<IEnumerable<OwnerResource>>> GETAllOwners()
        {
            try
            {
                var owners = await _serviceOwner.GetAllOwners();
                //--> Mpper permet de convertir la liste des Owner vers OwnerResource
                var ownerResources = _mapperService.Map<IEnumerable<Owner>, IEnumerable<OwnerResource>>(owners);
                return Ok(ownerResources);
            }
            catch (Exception ex)
            {

                return BadRequest(ex.Message);
            }
        }

        [HttpGet("{id}")]
      
        public async Task<ActionResult<OwnerResource>> GetOwner(Guid id)
        {
            try
            {
                var owner = await _serviceOwner.GetOwnerById(id);

                if (owner == null)
                    return NotFound();

                //--> Mpper permet de convertir la liste des Owner vers OwnerResource
                var ownerResource = _mapperService.Map<Owner, OwnerResource>(owner);

                return Ok(ownerResource);
            }
            catch (Exception ex)
            {

                return BadRequest(ex.Message);
            }
        }

        [HttpPost("")]
        [Authorize]
        public async Task<ActionResult<OwnerResource>> CreateOwner(SaveOwnerResource saveOwnerResource)
        {
            // Validation
            var validation = new SaveOwnerResourceValidator();
            var validationResult = await validation.ValidateAsync(saveOwnerResource);

            if (!validationResult.IsValid )
                return BadRequest(validationResult.Errors);

            // mappage de SaveOwnerResource vers Owner
            var owner = _mapperService.Map<SaveOwnerResource, Owner>(saveOwnerResource);

            // Creation newOwner
            var newOwner = await _serviceOwner.CreateOwner(owner);

            // mappage de Owner vers OwnerResource
            var ownerResource = _mapperService.Map<Owner, OwnerResource>(newOwner);
            return Ok(ownerResource);
        }

        [HttpPut("{id}")]
        [Authorize]
        public async Task<ActionResult<OwnerResource>> UpdateOwner(Guid id, SaveOwnerResource saveOwnerResource)
        {
            /// validation
            var validation = new SaveOwnerResourceValidator();
            var validationResult = await validation.ValidateAsync(saveOwnerResource);

            if (!validationResult.IsValid) return BadRequest(validationResult.Errors);

            // si la owner existe depuis le id
            var ownerUpdate = await _serviceOwner.GetOwnerById(id);
            if (ownerUpdate == null) return BadRequest("owner n'existe pas ");

            var owner = _mapperService.Map<SaveOwnerResource, Owner>(saveOwnerResource);

            await _serviceOwner.UpdateOwner(ownerUpdate, owner);
            var ownerUpdateNew = await _serviceOwner.GetOwnerById(id);
            var ownerResourceUpdate = _mapperService.Map<Owner, SaveOwnerResource>(ownerUpdateNew);

            return Ok(ownerResourceUpdate);
        }

        [HttpDelete("{id}")]
        [Authorize]
        public async Task<ActionResult> DeleteOwner(Guid id)
        {
            var owner = await _serviceOwner.GetOwnerById(id);
            if (owner == null) return BadRequest("Owner n'existe pas");

            await _serviceOwner.DeleteOwner(owner);
            return NoContent();
        }
    }
}
