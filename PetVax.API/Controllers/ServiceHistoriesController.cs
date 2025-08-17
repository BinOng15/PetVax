using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PediVax.BusinessObjects.DBContext;
using PetVax.BusinessObjects.DTO;
using PetVax.BusinessObjects.Models;
using PetVax.Services.IService;
using PetVax.Services.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static PetVax.BusinessObjects.Enum.EnumList;

namespace PediVax.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ServiceHistoriesController : ControllerBase
    {
        private readonly IServiceHistoryService _serviceHistoryService;

        public ServiceHistoriesController(IServiceHistoryService serviceHistoryService)
        {
            _serviceHistoryService = serviceHistoryService;
        }
        // GET: api/ServiceHistories/Customer/{customerId}
        [HttpGet("Get-service-history-by-customer/{customerId}")]
        public async Task<IActionResult> GetServiceHistoriesByCustomerId(int customerId, CancellationToken cancellationToken)
        {
            var response = await _serviceHistoryService.GetServiceHistoryByCustomerIdAsync(customerId, cancellationToken);
            return Ok(response);
        }

        // Get all service histories 
        [HttpGet("Get-all-service-histories")]
        public async Task<IActionResult> GetAllServiceHistories([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10, [FromQuery] string? keyWord = null, CancellationToken cancellationToken = default)
        {
            var request = new GetAllItemsDTO
            {
                PageNumber = pageNumber,
                PageSize = pageSize,
                KeyWord = keyWord
            };
            var response = await _serviceHistoryService.GetAllServiceHistoryAsync(request, cancellationToken);
            return StatusCode(response.Code, response);
        }

        [HttpGet("Get-service-history-by-type/{serviceType}")]
        public async Task<IActionResult> GetServiceHistoriesByServiceType(ServiceType serviceType, CancellationToken cancellationToken)
        {
            var response = await _serviceHistoryService.GetServiceHistoryByServiceTypedAsync(serviceType, cancellationToken);
            return Ok(response);
        }

    }
}
