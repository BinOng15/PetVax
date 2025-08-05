using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PetVax.Services.IService;

namespace PediVax.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DashboardController : ControllerBase
    {
        private readonly IDashboardService _dashboardService;
        public DashboardController(IDashboardService dashboardService)
        {
            _dashboardService = dashboardService;
        }
        [HttpGet("admin-dashboard")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetAdminDashboardData(CancellationToken cancellationToken)
        {
            var response = await _dashboardService.GetDashboardDataForAdminAsync(cancellationToken);
            return StatusCode(response.Code, response);
        }
        [HttpGet("vet-dashboard")]
        [Authorize(Roles = "Vet")]
        public async Task<IActionResult> GetVetDashboardData(CancellationToken cancellationToken)
        {
            var response = await _dashboardService.GetDashboardDataForVetAsync(cancellationToken);
            return StatusCode(response.Code, response);
        }
    }
}
