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
    public class ServiceItemController : ControllerBase
    {
        private readonly IServiceItemService _serviceServiceItem;
        private readonly IMapper _mapperService;

        public ServiceItemController(IServiceItemService serviceServiceItem, IMapper mapperService)
        {
            _serviceServiceItem = serviceServiceItem;
            _mapperService = mapperService;

        }

        [HttpGet("")]
        public async Task<ActionResult<IEnumerable<ServiceItemResource>>> GETAllServiceItems()
        {
            try
            {
                var serviceItems = await _serviceServiceItem.GetAllServiceItems();
                //--> Mpper permet de convertir la liste des ServiceItem vers ServiceItemResource
                var serviceItemResources = _mapperService.Map<IEnumerable<ServiceItem>, IEnumerable<ServiceItemResource>>(serviceItems);
                return Ok(serviceItemResources);
            }
            catch (Exception ex)
            {

                return BadRequest(ex.Message);
            }
        }

        [HttpGet("{id}")]
       
        public async Task<ActionResult<ServiceItemResource>> GetServiceItem(Guid id)
        {
            try
            {
                var serviceItem = await _serviceServiceItem.GetServiceItemById(id);

                if (serviceItem == null)
                    return NotFound();

                //--> Mpper permet de convertir la liste des ServiceItem vers ServiceItemResource
                var serviceItemResource = _mapperService.Map<ServiceItem, ServiceItemResource>(serviceItem);

                return Ok(serviceItemResource);
            }
            catch (Exception ex)
            {

                return BadRequest(ex.Message);
            }
        }

        [HttpPost("")]
        [Authorize]
        public async Task<ActionResult<ServiceItemResource>> CreateServiceItem(SaveServiceItemResource saveServiceItemResource)
        {
            // Validation
            var validation = new SaveServiceItemResourceValidator();
            var validationResult = await validation.ValidateAsync(saveServiceItemResource);

            if (!validationResult.IsValid)
                return BadRequest(validationResult.Errors);

            // mappage de SaveServiceItemResource vers ServiceItem
            var serviceItem = _mapperService.Map<SaveServiceItemResource, ServiceItem>(saveServiceItemResource);

            // Creation newServiceItem
            var newServiceItem = await _serviceServiceItem.CreateServiceItem(serviceItem);

            // mappage de ServiceItem vers ServiceItemResource
            var serviceItemResource = _mapperService.Map<ServiceItem, ServiceItemResource>(newServiceItem);
            return Ok(serviceItemResource);
        }

        [HttpPut("{id}")]
        [Authorize]
        public async Task<ActionResult<ServiceItemResource>> UpdateServiceItem(Guid id, SaveServiceItemResource saveServiceItemResource)
        {
            /// validation
            var validation = new SaveServiceItemResourceValidator();
            var validationResult = await validation.ValidateAsync(saveServiceItemResource);

            if (!validationResult.IsValid) return BadRequest(validationResult.Errors);

            // si la ServiceItem existe depuis le id
            var serviceItemUpdate = await _serviceServiceItem.GetServiceItemById(id);
            if (serviceItemUpdate == null) return BadRequest("Service n'existe pas ");

            var serviceItem = _mapperService.Map<SaveServiceItemResource, ServiceItem>(saveServiceItemResource);

            await _serviceServiceItem.UpdateServiceItem(serviceItemUpdate, serviceItem);
            var serviceItemUpdateNew = await _serviceServiceItem.GetServiceItemById(id);
            var serviceItemResourceUpdate = _mapperService.Map<ServiceItem, SaveServiceItemResource>(serviceItemUpdateNew);

            return Ok(serviceItemResourceUpdate);
        }

        [HttpDelete("{id}")]
        [Authorize]
        public async Task<ActionResult> DeleteServiceItem(Guid id)
        {
            var serviceItem = await _serviceServiceItem.GetServiceItemById(id);
            if (serviceItem == null) return BadRequest("Service n'existe pas");

            await _serviceServiceItem.DeleteServiceItem(serviceItem);
            return NoContent();
        }
    }
}
