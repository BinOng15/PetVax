using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PetVax.BusinessObjects.DTO;
using PetVax.BusinessObjects.DTO.VoucherDTO;
using PetVax.Services.IService;
using PetVax.Services.Service;

namespace PediVax.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VoucherController : ControllerBase
    {
        private readonly IVoucherService _voucherService;

        public VoucherController(IVoucherService voucherService)
        {
            _voucherService = voucherService;
        }

        [HttpGet("get-all-vouchers")]
        public async Task<IActionResult> GetAllVouchersAsync([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10, [FromQuery] string? keyWord = null, [FromQuery] bool? status = true, CancellationToken cancellationToken = default)
        {
            var getAllItemsDTO = new GetAllItemsDTO
            {
                PageNumber = pageNumber,
                PageSize = pageSize,
                KeyWord = keyWord,
                Status = status
            };
            var response = await _voucherService.GetAllVoucherAsync(getAllItemsDTO, cancellationToken);
            return StatusCode(response.Code, response);
        }
        [HttpGet("get-voucher-by-id/{voucherId}")]
        public async Task<IActionResult> GetVoucherByIdAsync(int voucherId, CancellationToken cancellationToken = default)
        {
            var response = await _voucherService.GetVoucherByIdAsync(voucherId, cancellationToken);
            return StatusCode(response.Code, response);
        }
        [HttpGet("get-voucher-by-code/{voucherCode}")]
        public async Task<IActionResult> GetVoucherByCodeAsync(string voucherCode, CancellationToken cancellationToken = default)
        {
            var response = await _voucherService.GetVoucherByCodeAsync(voucherCode, cancellationToken);
            return StatusCode(response.Code, response);
        }
        [HttpGet("get-voucher-by-transaction-id/{transactionId}")]
        public async Task<IActionResult> GetVoucherByTransactionIdAsync(int transactionId, CancellationToken cancellationToken = default)
        {
            var response = await _voucherService.GetVoucherByTransactionIdAsync(transactionId, cancellationToken);
            return StatusCode(response.Code, response);
        }
        [HttpPost("create-voucher")]
        public async Task<IActionResult> CreateVoucherAsync([FromBody] CreateVoucherDTO createVoucherDTO, CancellationToken cancellationToken = default)
        {
            var response = await _voucherService.CreateVoucherAsync(createVoucherDTO, cancellationToken);
            return StatusCode(response.Code, response);
        }
        [HttpPut("update-voucher/{voucherId}")]
        public async Task<IActionResult> UpdateVoucherAsync(int voucherId, [FromBody] UpdateVoucherDTO updateVoucherDTO, CancellationToken cancellationToken = default)
        {
            var response = await _voucherService.UpdateVoucherAsync(voucherId, updateVoucherDTO, cancellationToken);
            return StatusCode(response.Code, response);
        }
        [HttpDelete("delete-voucher/{voucherId}")]
        public async Task<IActionResult> DeleteVoucherAsync(int voucherId, CancellationToken cancellationToken = default)
        {
            var response = await _voucherService.DeleteVoucherAsync(voucherId, cancellationToken);
            return StatusCode(response.Code, response);
        }
        [HttpPost("redeem-points-for-voucher/{customedId}/{voucherId}")]
        public async Task<IActionResult> RedeemPointsForVoucherAsync(int customedId, int voucherId, CancellationToken cancellationToken = default)
        {
            var response = await _voucherService.RedeemPointsForVoucherAsync(customedId, voucherId, cancellationToken);
            return StatusCode(response.Code, response);
        }
    }
}
