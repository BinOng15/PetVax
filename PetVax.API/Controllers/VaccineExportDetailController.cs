using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PetVax.BusinessObjects.DTO;
using PetVax.BusinessObjects.DTO.VaccineExportDetailDTO;
using PetVax.Services.IService;
using PetVax.Services.Service;

namespace PediVax.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VaccineExportDetailController : ControllerBase
    {
        private readonly IVaccineExportDetailService _vaccineExportDetailService;

        public VaccineExportDetailController(IVaccineExportDetailService vaccineExportDetailService)
        {
            _vaccineExportDetailService = vaccineExportDetailService;
        }

        [HttpGet("get-all-vaccine-export-details")]
        [Authorize(Roles = "Admin, Staff")]
        public async Task<IActionResult> GetAllVaccineExportDetails([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10, [FromQuery] string? keyword = null, [FromQuery] bool? status = true, CancellationToken cancellationToken = default)
        {
            var getAllItemsDTO = new GetAllItemsDTO
            {
                PageNumber = pageNumber,
                PageSize = pageSize,
                KeyWord = keyword,
                Status = status
            };
            var response = await _vaccineExportDetailService.GetAllVaccineExportDetailsAsync(getAllItemsDTO, cancellationToken);
            return StatusCode(response.Code, response);
        }
        [HttpGet("get-vaccine-export-detail-by-id/{vaccineExportDetailId}")]
        [Authorize(Roles = "Admin, Staff, Vet")]
        public async Task<IActionResult> GetVaccineExportDetailById(int vaccineExportDetailId, CancellationToken cancellationToken = default)
        {
            var response = await _vaccineExportDetailService.GetVaccineExportDetailByIdAsync(vaccineExportDetailId, cancellationToken);
            return StatusCode(response.Code, response);
        }
        [HttpPost("create-vaccine-export-detail")]
        [Authorize(Roles = "Admin, Staff")]
        public async Task<IActionResult> CreateVaccineExportDetail([FromBody] CreateVaccineExportDetailDTO vaccineExportDetailDTO, CancellationToken cancellationToken = default)
        {
            var response = await _vaccineExportDetailService.CreateVaccineExportDetailAsync(vaccineExportDetailDTO, cancellationToken);
            return StatusCode(response.Code, response);
        }
        [HttpPut("update-vaccine-export-detail")]
        [Authorize(Roles = "Admin, Staff")]
        public async Task<IActionResult> UpdateVaccineExportDetail(int exportDetailId, [FromBody] UpdateVaccineExportDetailDTO vaccineExportDetailDTO, CancellationToken cancellationToken = default)
        {
            var response = await _vaccineExportDetailService.UpdateVaccineExportDetailAsync(exportDetailId, vaccineExportDetailDTO, cancellationToken);
            return StatusCode(response.Code, response);
        }
        [HttpDelete("delete-vaccine-export-detail/{vaccineExportDetailId}")]
        [Authorize(Roles = "Admin, Staff")]
        public async Task<IActionResult> DeleteVaccineExportDetail(int vaccineExportDetailId, CancellationToken cancellationToken = default)
        {
            var response = await _vaccineExportDetailService.DeleteVaccineExportDetailAsync(vaccineExportDetailId, cancellationToken);
            return StatusCode(response.Code, response);
        }
        [HttpGet("get-vaccine-export-details-by-vaccine-batch-id/{vaccineBatchId}")]
        [Authorize(Roles = "Admin, Staff, Vet")]
        public async Task<IActionResult> GetVaccineExportDetailsByVaccineBatchId(int vaccineBatchId, CancellationToken cancellationToken = default)
        {
            var response = await _vaccineExportDetailService.GetVaccineExportDetailByVaccineBatchIdAsync(vaccineBatchId, cancellationToken);
            return StatusCode(response.Code, response);
        }
        [HttpGet("get-vaccine-export-details-by-vaccine-export-id/{vaccineExportId}")]
        [Authorize(Roles = "Admin, Staff, Vet")]
        public async Task<IActionResult> GetVaccineExportDetailsByVaccineExportId(int vaccineExportId, CancellationToken cancellationToken = default)
        {
            var response = await _vaccineExportDetailService.GetVaccineExportDetailByVaccineExportIdAsync(vaccineExportId, cancellationToken);
            return StatusCode(response.Code, response);
        }
        [HttpPost("create-vaccine-export-detail-for-vaccination")]
        [Authorize(Roles = "Admin, Staff, Vet")]
        public async Task<IActionResult> CreateVaccineExportDetailForVaccination([FromBody] CreateVaccineExportDetailForVaccinationDTO vaccineExportDetailDTO, CancellationToken cancellationToken = default)
        {
            var response = await _vaccineExportDetailService.CreateVaccineExportDetailForVaccinationAsync(vaccineExportDetailDTO, cancellationToken);
            return StatusCode(response.Code, response);
        }
        [HttpPut("update-vaccine-export-detail-for-vaccination")]
        [Authorize(Roles = "Admin, Staff, Vet")]
        public async Task<IActionResult> UpdateVaccineExportDetailForVaccination(int exportDetailId, [FromBody] UpdateVaccineExportDetailForVaccinationDTO vaccineExportDetailDTO, CancellationToken cancellationToken = default)
        {
            var response = await _vaccineExportDetailService.UpdateVaccineExportDetailForVaccinationAsync(exportDetailId, vaccineExportDetailDTO, cancellationToken);
            return StatusCode(response.Code, response);
        }
        [HttpGet("get-vaccine-export-detail-by-appointment-detail-id/{appointmentDetailId}")]
        [Authorize(Roles = "Admin, Staff, Vet")]
        public async Task<IActionResult> GetVaccineExportDetailByAppointmentDetailId(int appointmentDetailId, CancellationToken cancellationToken = default)
        {
            var response = await _vaccineExportDetailService.GetVaccineExportDetailByAppointmentDetailIdAsync(appointmentDetailId, cancellationToken);
            return StatusCode(response.Code, response);
        }
        [HttpPost("create-full-vaccine-export")]
        [Authorize(Roles = "Admin, Staff")]
        public async Task<IActionResult> CreateFullVaccineExport([FromBody] CreateFullVaccineExportDTO createFullVaccineExportDTO, CancellationToken cancellationToken = default)
        {
            var response = await _vaccineExportDetailService.CreateFullVaccineExportAsync(createFullVaccineExportDTO, cancellationToken);
            return StatusCode(response.Code, response);
        }
        [HttpGet("get-list-vaccine-export-detail-by-vaccine-batch-id/{vaccineBatchId}")]
        [Authorize(Roles = "Admin, Staff, Vet")]
        public async Task<IActionResult> GetListVaccineExportDetailByVaccineBatchId(int vaccineBatchId, CancellationToken cancellationToken = default)
        {
            var response = await _vaccineExportDetailService.GetListVaccineExportDetailByVaccineBatchIdAsync(vaccineBatchId, cancellationToken);
            return StatusCode(response.Code, response);
        }
    }
}
