using PetVax.BusinessObjects.DTO;
using PetVax.BusinessObjects.DTO.PaymentDTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static PetVax.BusinessObjects.DTO.ResponseModel;

namespace PetVax.Services.IService
{
    public interface IPaymentService
    {
        Task<DynamicResponse<PaymentResponseDTO>> GetAllPaymentsAsync(GetAllItemsDTO getAllItemsRequest, CancellationToken cancellationToken);
        Task<BaseResponse<PaymentResponseDTO>> GetPaymentByIdAsync(int paymentId, CancellationToken cancellationToken);
        Task<BaseResponse<PaymentResponseDTO>> CreatePaymentAsync(CreatePaymentRequestDTO createPaymentRequest, CancellationToken cancellationToken);
        Task<BaseResponse<PaymentResponseDTO>> UpdatePaymentAsync(int paymentId, UpdatePaymentRequestDTO updatePaymentRequest, CancellationToken cancellationToken);
        Task<BaseResponse<PaymentResponseDTO>> DeletePaymentByIdAsync(int paymentId, CancellationToken cancellationToken);
        Task<BaseResponse<List<PaymentResponseDTO>>> GetPaymentsByCustomerIdAsync(int customerId, CancellationToken cancellationToken);
        Task<BaseResponse<List<PaymentResponseDTO>>> GetPaymentsByAppointmentDetailIdAsync(int appointmentDetailId, CancellationToken cancellationToken);
        Task<BaseResponse<PaymentResponseDTO>> HandlePayOsCallBackAsync(PaymentCallBackDTO paymentCallBackDTO, CancellationToken cancellationToken);
        Task<BaseResponse<PaymentResponseDTO>> RetryPaymentAsync(RetryPaymentRequestDTO retryPaymentRequest, CancellationToken cancellationToken);
    }
}
