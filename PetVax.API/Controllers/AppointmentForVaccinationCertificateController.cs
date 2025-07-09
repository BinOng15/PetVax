//using Microsoft.AspNetCore.Http;
//using Microsoft.AspNetCore.Mvc;
//using PetVax.BusinessObjects.DTO;
//using PetVax.BusinessObjects.DTO.AppointmentDetailDTO;
//using PetVax.BusinessObjects.DTO.AppointmentDTO;
//using PetVax.Services.IService;

//namespace PediVax.Controllers
//{
//    [Route("api/[controller]")]
//    [ApiController]
//    public class AppointmentForVaccinationCertificateController : ControllerBase
//    {
//        private readonly IAppointmentService _appointmentService;
//        private readonly IAppointmentDetailService _appointmentDetailService;

//        public AppointmentForVaccinationCertificateController(
//            IAppointmentService appointmentService,
//            IAppointmentDetailService appointmentDetailService,
//            ILogger<AppointmentForVaccinationCertificateController> logger)
//        {
//            _appointmentService = appointmentService;
//            _appointmentDetailService = appointmentDetailService;
//        }
//        [HttpGet("get-all")]
//        public async Task<IActionResult> GetAllAppointmentVaccinationCertificateAsync([FromQuery] GetAllItemsDTO getAllItemsDTO, CancellationToken cancellationToken)
//        {
//            var response = await _appointmentService.GetAllAppointmentVaccinationCertificateAsync(getAllItemsDTO, cancellationToken);
//            return StatusCode(response.Code, response);
//        }
//        [HttpGet("get-by-id/{appointmentId}")]
//        public async Task<IActionResult> GetAppointmentVaccinationeCertificateByIdAsync(int appointmentId, CancellationToken cancellationToken)
//        {
//            var response = await _appointmentService.GetAppointmentVaccinationCertificateById(appointmentId, cancellationToken);
//            return StatusCode(response.Code, response);
//        }
//        [HttpPost("create")]
//        public async Task<IActionResult> CreateAppointmentVaccinationCertificateAsync([FromBody] CreateAppointmentVaccinationCertificateDTO createAppointmentVaccinationCertificateDTO, CancellationToken cancellationToken)
//        {
//            var response = await _appointmentService.CreateAppointmentVaccinationCertificate(createAppointmentVaccinationCertificateDTO, cancellationToken);
//            return StatusCode(response.Code, response);
//        }
//        [HttpPut("update/{appointmentId}")]
//        public async Task<IActionResult> UpdateAppointmentVaccinationCertificateAsync(int appointmentId, [FromBody] UpdateAppointmentDTO updateAppointmentDTO, CancellationToken cancellationToken)
//        {
//            var response = await _appointmentService.UpdateAppointmentVaccinationCertificate(appointmentId, updateAppointmentDTO, cancellationToken);
//            return StatusCode(response.Code, response);
//        }
//        [HttpPut("update-detail/{appointmentId}")]
//        public async Task<IActionResult> UpdateAppointmentDetailVaccinationCertificateAsync(int appointmentId, [FromBody] UpdateAppointmentVaccinationCertificateDTO updateAppointmentVaccinationCertificateDTO, CancellationToken cancellationToken)
//        {
//            var response = await _appointmentService.UpdateAppointmentDetailVaccinationCertificate(appointmentId, updateAppointmentVaccinationCertificateDTO, cancellationToken);
//            return StatusCode(response.Code, response);
//        }
//    }
//}
