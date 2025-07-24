using PetVax.BusinessObjects.DTO;
using PetVax.BusinessObjects.DTO.PointTransactionDTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static PetVax.BusinessObjects.DTO.ResponseModel;

namespace PetVax.Services.IService
{
    public interface IPointTransactionService
    {
        Task<DynamicResponse<PointTransactionResponseDTO>> GetAllPointTransactionsAsync(GetAllItemsDTO getAllItemsDTO, CancellationToken cancellationToken);
        Task<BaseResponse<PointTransactionResponseDTO>> GetPointTransactionByIdAsync(int pointTransactionId, CancellationToken cancellationToken);
        Task<BaseResponse<List<PointTransactionResponseDTO>>> GetPointTransactionByCustomerIdAsync(int customerId, CancellationToken cancellationToken);
    }
}
