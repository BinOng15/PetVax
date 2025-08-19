using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PetVax.BusinessObjects.DTO;
using PetVax.BusinessObjects.DTO.ColdChainLogDTO;
using PetVax.Services.IService;
using PetVax.Services.Service;

namespace PediVax.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ColdChainLogController : ControllerBase
    {
        private readonly IColdChainLogService _coldChainLogService;

        public ColdChainLogController(IColdChainLogService coldChainLogService)
        {
            _coldChainLogService = coldChainLogService;
        }

        [HttpGet("get-all-cold-chain-logs")]
        public async Task<IActionResult> GetAllColdChainLogs([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10, [FromQuery] string? keyword = null, [FromQuery] bool? status = true, CancellationToken cancellationToken = default)
        {
            var getAllItemsDTO = new GetAllItemsDTO
            {
                PageNumber = pageNumber,
                PageSize = pageSize,
                KeyWord = keyword,
                Status = status
            };
            var response = await _coldChainLogService.GetAllColdChainLogsAsync(getAllItemsDTO, cancellationToken);
            return StatusCode(response.Code, response);
        }
        [HttpGet("get-cold-chain-log-by-id/{coldChainLogId}")]
        public async Task<IActionResult> GetColdChainLogById(int coldChainLogId, CancellationToken cancellationToken = default)
        {
            var response = await _coldChainLogService.GetColdchainLogById(coldChainLogId, cancellationToken);
            return StatusCode(response.Code, response);
        }
        //[HttpPost("create-cold-chain-log")]
        //public async Task<IActionResult> CreateColdChainLog([FromBody] CreateColdChainLogDTO createColdChainLogDTO, CancellationToken cancellationToken = default)
        //{
        //    var response = await _coldChainLogService.CreateColdChainLogAsync(createColdChainLogDTO, cancellationToken);
        //    return StatusCode(response.Code, response);
        //}
        [HttpPut("update-cold-chain-log/{coldChainLogId}")]
        public async Task<IActionResult> UpdateColdChainLog(int coldChainLogId, [FromBody] UpdateColdChainLogDTO updateColdChainLogDTO, CancellationToken cancellationToken = default)
        {
            var response = await _coldChainLogService.UpdateColdChainLogAsync(coldChainLogId, updateColdChainLogDTO, cancellationToken);
            return StatusCode(response.Code, response);
        }
        [HttpDelete("delete-cold-chain-log/{coldChainLogId}")]
        public async Task<IActionResult> DeleteColdChainLog(int coldChainLogId, CancellationToken cancellationToken = default)
        {
            var response = await _coldChainLogService.DeleteColdChainLogAsync(coldChainLogId, cancellationToken);
            return StatusCode(response.Code, response);
        }
        [HttpGet("get-cold-chain-logs-by-vaccine-batch-id/{vaccineBatchId}")]
        public async Task<IActionResult> GetListColdChainLogsByVaccineBatchId(int vaccineBatchId, CancellationToken cancellationToken = default)
        {
            var response = await _coldChainLogService.GetColdChainLogsByVaccineBatchIdAsync(vaccineBatchId, new GetAllItemsDTO(), cancellationToken);
            return StatusCode(response.Code, response);
        }
    }
}
