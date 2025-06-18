using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PetVax.BusinessObjects.DTO;
using PetVax.BusinessObjects.DTO.AppointmentDetailDTO;
using PetVax.BusinessObjects.Enum;
using PetVax.Services.IService;

namespace PediVax.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AppointmentDetailController : ControllerBase
    {
        private readonly IAppointmentDetailService _appointmentDetailService;
        private readonly ILogger<AppointmentDetailController> _logger;

        public AppointmentDetailController(IAppointmentDetailService appointmentDetailService, ILogger<AppointmentDetailController> logger)
        {
            _appointmentDetailService = appointmentDetailService;
            _logger = logger;
        }

        [HttpGet("get-all-appointment-details")]
        public async Task<IActionResult> GetAllAppointmentDetails([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10, [FromQuery] string? keyWord = null, CancellationToken cancellationToken = default)
        {
            var request = new GetAllItemsDTO
            {
                PageNumber = pageNumber,
                PageSize = pageSize,
                KeyWord = keyWord
            };
            var response = await _appointmentDetailService.GetAllAppointmentDetail(request, cancellationToken);
            return StatusCode(response.Code, response);
        }
        [HttpGet("get-appointment-detail-by-id/{appointmentDetailId}")]
        public async Task<IActionResult> GetAppointmentDetailById(int appointmentDetailId, CancellationToken cancellationToken)
        {
            var response = await _appointmentDetailService.GetAppointmentDetailById(appointmentDetailId, cancellationToken);
            return StatusCode(response.Code, response);
        }
        [HttpGet("get-appointment-detail-by-appointment-id/{appointmentId}")]
        public async Task<IActionResult> GetAppointmentDetailByAppointmentId(int appointmentId, CancellationToken cancellationToken)
        {
            var response = await _appointmentDetailService.GetAppointmentDetailByAppointmentId(appointmentId, cancellationToken);
            return StatusCode(response.Code, response);
        }
        [HttpGet("get-appointment-detail-by-pet-id/{petId}")]
        public async Task<IActionResult> GetAppointmentDetailByPetId(int petId, CancellationToken cancellationToken)
        {
            var response = await _appointmentDetailService.GetAppointmentDetailByPetId(petId, cancellationToken);
            return StatusCode(response.Code, response);
        }
        [HttpGet("get-appointment-detail-by-vet-id/{vetId}")]
        public async Task<IActionResult> GetAppointmentDetailByVetId(int vetId, CancellationToken cancellationToken)
        {
            var response = await _appointmentDetailService.GetAppointmentDetailByVetId(vetId, cancellationToken);
            return StatusCode(response.Code, response);
        }
        [HttpGet("get-appointment-detail-by-service-type/{serviceType}")]
        public async Task<IActionResult> GetAppointmentDetailByServiceType(EnumList.ServiceType serviceType, CancellationToken cancellationToken)
        {
            var response = await _appointmentDetailService.GetAppointmentDetailByServiceType(serviceType, cancellationToken);
            return StatusCode(response.Code, response);
        }
        [HttpGet("get-appointment-detail-by-status/{status}")]
        public async Task<IActionResult> GetAppointmentDetailByStatus(EnumList.AppointmentStatus status, CancellationToken cancellationToken)
        {
            var response = await _appointmentDetailService.GetAppointmentDetailByStatus(status, cancellationToken);
            return StatusCode(response.Code, response);
        }
        //[HttpPost("create-appointment-detail")]
        //public async Task<IActionResult> CreateAppointmentDetail([FromBody] CreateAppointmentDetailDTO createAppointmentDetailDTO, CancellationToken cancellationToken)
        //{
        //    var response = await _appointmentDetailService.CreateAppointmentDetail(createAppointmentDetailDTO, cancellationToken);
        //    return StatusCode(response.Code, response);
        //}
        //[HttpPut("update-appointment-detail/{appointmentDetailId}")]
        //public async Task<IActionResult> UpdateAppointmentDetail(int appointmentDetailId, [FromBody] UpdateAppointmentDetailDTO updateAppointmentDetailDTO, CancellationToken cancellationToken)
        //{
        //    var response = await _appointmentDetailService.UpdateAppointmentDetail(appointmentDetailId, updateAppointmentDetailDTO, cancellationToken);
        //    return StatusCode(response.Code, response);
        //}
        [HttpDelete("delete-appointment-detail/{appointmentDetailId}")]
        public async Task<IActionResult> DeleteAppointmentDetail(int appointmentDetailId, CancellationToken cancellationToken)
        {
            var response = await _appointmentDetailService.DeleteAppointmentDetail(appointmentDetailId, cancellationToken);
            return StatusCode(response.Code, response);
        }
    }
}
