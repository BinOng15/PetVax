using AutoMapper;
using Microsoft.Extensions.Logging;
using PetVax.BusinessObjects.DTO;
using PetVax.BusinessObjects.DTO.DashboardDTO;
using PetVax.BusinessObjects.Helpers;
using PetVax.Repositories.IRepository;
using PetVax.Services.IService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static PetVax.BusinessObjects.DTO.ResponseModel;

namespace PetVax.Services.Service
{
    public class DashboardService : IDashboardService
    {
        private readonly IAccountRepository _accountRepository;
        private readonly IAppointmentRepository _appointmentRepository;
        private readonly IAppointmentDetailRepository _appointmentDetailRepository;
        private readonly ICustomerRepository _customerRepository;
        private readonly IDiseaseRepository _diseaseRepository;
        private readonly IHealthConditionRepository _healthConditionRepository;
        private readonly IMembershipRepository _membershipRepository;
        private readonly IMicrochipRepository _microchipRepository;
        private readonly IMicrochipItemRepository _microchipItemRepository;
        private readonly IPaymentRepository _paymentRepository;
        private readonly IPetRepository _petRepository;
        private readonly IVaccineBatchRepository _vaccineBatchRepository;
        private readonly IVaccineExportDetailRepository _vaccineExportDetailRepository;
        private readonly IVaccineExportRepository _vaccineExportRepository;
        private readonly IVaccineReceiptDetailRepository _vaccineReceiptDetailRepository;
        private readonly IVaccineReceiptRepository _vaccineReceiptRepository;
        private readonly IVaccineRepository _vaccineRepository;
        private readonly IVetRepository _vetRepository;
        private readonly IVetScheduleRepository _vetScheduleRepository;
        private readonly IVoucherRepository _voucherRepository;
        private readonly ILogger<DashboardService> _logger;
        private readonly IMapper _mapper;

        public DashboardService(
            IAccountRepository accountRepository,
            IAppointmentRepository appointmentRepository,
            IAppointmentDetailRepository appointmentDetailRepository,
            ICustomerRepository customerRepository,
            IDiseaseRepository diseaseRepository,
            IHealthConditionRepository healthConditionRepository,
            IMembershipRepository membershipRepository,
            IMicrochipRepository microchipRepository,
            IMicrochipItemRepository microchipItemRepository,
            IPaymentRepository paymentRepository,
            IPetRepository petRepository,
            IVaccineBatchRepository vaccineBatchRepository,
            IVaccineExportDetailRepository vaccineExportDetailRepository,
            IVaccineExportRepository vaccineExportRepository,
            IVaccineReceiptDetailRepository vaccineReceiptDetailRepository,
            IVaccineReceiptRepository vaccineReceiptRepository,
            IVaccineRepository vaccineRepository,
            IVetRepository vetRepository,
            IVetScheduleRepository vetScheduleRepository,
            IVoucherRepository voucherRepository,
            ILogger<DashboardService> logger,
            IMapper mapper)
        {
            _accountRepository = accountRepository;
            _appointmentRepository = appointmentRepository;
            _appointmentDetailRepository = appointmentDetailRepository;
            _customerRepository = customerRepository;
            _diseaseRepository = diseaseRepository;
            _healthConditionRepository = healthConditionRepository;
            _membershipRepository = membershipRepository;
            _microchipRepository = microchipRepository;
            _microchipItemRepository = microchipItemRepository;
            _paymentRepository = paymentRepository;
            _petRepository = petRepository;
            _vaccineBatchRepository = vaccineBatchRepository;
            _vaccineExportDetailRepository = vaccineExportDetailRepository;
            _vaccineExportRepository = vaccineExportRepository;
            _vaccineReceiptDetailRepository = vaccineReceiptDetailRepository;
            _vaccineReceiptRepository = vaccineReceiptRepository;
            _vaccineRepository = vaccineRepository;
            _vetScheduleRepository = vetScheduleRepository;
            _vetRepository = vetRepository;
            _voucherRepository = voucherRepository;

            _logger = logger;
            _mapper = mapper;
        }

        public async Task<BaseResponse<AdminDashboardResponseDTO>> GetDashboardDataForAdminAsync(CancellationToken cancellationToken)
        {
            try
            {
                var totalAccounts = await _accountRepository.GetTotalAccounts(cancellationToken);
                var totalActiveAccounts = await _accountRepository.GetTotalActiveAccounts(cancellationToken);
                var totalCustomers = await _customerRepository.GetTotalCustomersAsync(cancellationToken);
                var totalDiseases = await _diseaseRepository.GetTotalDiseasesAsync(cancellationToken);
                var totalMemberships = await _membershipRepository.GetTotalMembershipsAsync(cancellationToken);
                var totalMicrochips = await _microchipRepository.GetTotalMicrochipsAsync(cancellationToken);
                var totalMicrochipItems = await _microchipItemRepository.GetTotalMicrochipItemsAsync(cancellationToken);
                var totalHealthConditions = await _healthConditionRepository.GetTotalHealthConditionsAsync(cancellationToken);
                var totalPets = await _petRepository.GetTotalPetsAsync(cancellationToken);
                var totalPayments = await _paymentRepository.GetTotalPaymentsAsync(cancellationToken);
                var totalVaccines = await _vaccineRepository.GetTotalVaccinesAsync(cancellationToken);
                var totalVaccineBatches = await _vaccineBatchRepository.GetTotalVaccineBatchesAsync(cancellationToken);
                var totalVaccineExports = await _vaccineExportRepository.GetTotalVaccineExportsAsync(cancellationToken);
                var totalVaccineExportDetails = await _vaccineExportDetailRepository.GetTotalVaccineExportDetailsAsync(cancellationToken);
                var totalVaccineReceipts = await _vaccineReceiptRepository.GetTotalVaccineReceiptsAsync(cancellationToken);
                var totalVaccineReceiptDetails = await _vaccineReceiptDetailRepository.GetTotalVaccineReceiptDetailsAsync(cancellationToken);
                var totalVets = await _vetRepository.GetTotalVetsAsync(cancellationToken);
                var totalVetSchedules = await _vetScheduleRepository.GetTotalVetSchedulesAsync(cancellationToken);
                var totalVouchers = await _voucherRepository.GetTotalVouchersAsync(cancellationToken);
                var totalAppointmentVaccinations = await _appointmentRepository.GetTotalAppointmentVaccinations(cancellationToken);
                var totalProcessingAppointmentVaccinations = await _appointmentRepository.GetTotalProcessingAppointmentVaccinations(cancellationToken);
                var totalConfirmedAppointmentVaccinations = await _appointmentRepository.GetTotalConfirmedAppointmentVaccinations(cancellationToken);
                var totalCheckedInAppointmentVaccinations = await _appointmentRepository.GetTotalCheckedInAppointmentVaccinations(cancellationToken);
                var totalPaidAppointmentVaccinations = await _appointmentRepository.GetTotalPaidAppointmentVaccinations(cancellationToken);
                var totalCompletedAppointmentVaccinations = await _appointmentRepository.GetTotalCompletedAppointmentVaccinations(cancellationToken);
                var totalCancelledAppointmentVaccinations = await _appointmentRepository.GetTotalCancelledAppointmentVaccinations(cancellationToken);
                var totalRejectedAppointmentVaccinations = await _appointmentRepository.GetTotalRejectedAppointmentVaccinations(cancellationToken);
                var totalAppointmentMicrochips = await _appointmentRepository.GetTotalAppointmentMicrochips(cancellationToken);
                var totalProcessingAppointmentMicrochips = await _appointmentRepository.GetTotalProcessingAppointmentMicrochips(cancellationToken);
                var totalConfirmedAppointmentMicrochips = await _appointmentRepository.GetTotalConfirmedAppointmentMicrochips(cancellationToken);
                var totalCheckedInAppointmentMicrochips = await _appointmentRepository.GetTotalCheckedInAppointmentMicrochips(cancellationToken);
                var totalPaidAppointmentMicrochips = await _appointmentRepository.GetTotalPaidAppointmentMicrochips(cancellationToken);
                var totalCompletedAppointmentMicrochips = await _appointmentRepository.GetTotalCompletedAppointmentMicrochips(cancellationToken);
                var totalCancelledAppointmentMicrochips = await _appointmentRepository.GetTotalCancelledAppointmentMicrochips(cancellationToken);
                var totalRejectedAppointmentMicrochips = await _appointmentRepository.GetTotalRejectedAppointmentMicrochips(cancellationToken);
                var totalAppointmentHealthConditions = await _appointmentRepository.GetTotalAppointmentHealthConditions(cancellationToken);
                var totalProcessingAppointmentHealthConditions = await _appointmentRepository.GetTotalProcessingAppointmentHealthConditions(cancellationToken);
                var totalConfirmedAppointmentHealthConditions = await _appointmentRepository.GetTotalConfirmedAppointmentHealthConditions(cancellationToken);
                var totalCheckedInAppointmentHealthConditions = await _appointmentRepository.GetTotalCheckedInAppointmentHealthConditions(cancellationToken);
                var totalPaidAppointmentHealthConditions = await _appointmentRepository.GetTotalPaidAppointmentHealthConditions(cancellationToken);
                var totalCompletedAppointmentHealthConditions = await _appointmentRepository.GetTotalCompletedAppointmentHealthConditions(cancellationToken);
                var totalCancelledAppointmentHealthConditions = await _appointmentRepository.GetTotalCancelledAppointmentHealthConditions(cancellationToken);
                var totalRejectedAppointmentHealthConditions = await _appointmentRepository.GetTotalRejectedAppointmentHealthConditions(cancellationToken);

                var dashboardData = new AdminDashboardResponseDTO
                {
                    TotalAccounts = totalAccounts,
                    TotalActiveAccounts = totalActiveAccounts,
                    TotalCustomers = totalCustomers,
                    TotalDiseases = totalDiseases,
                    TotalMemberships = totalMemberships,
                    TotalMicrochips = totalMicrochips,
                    TotalMicrochipItems = totalMicrochipItems,
                    TotalHealthConditions = totalHealthConditions,
                    TotalPets = totalPets,
                    TotalPayments = totalPayments,
                    TotalVaccines = totalVaccines,
                    TotalVaccineBatches = totalVaccineBatches,
                    TotalVaccineExports = totalVaccineExports,
                    TotalVaccineExportDetails = totalVaccineExportDetails,
                    TotalVaccineReceipts = totalVaccineReceipts,
                    TotalVaccineReceiptDetails = totalVaccineReceiptDetails,
                    TotalVets = totalVets,
                    TotalVetSchedules = totalVetSchedules,
                    TotalVouchers = totalVouchers,
                    TotalAppointmentVaccinations = totalAppointmentVaccinations,
                    TotalProcessingAppointmentVaccinations = totalProcessingAppointmentVaccinations,
                    TotalConfirmedAppointmentVaccinations = totalConfirmedAppointmentVaccinations,
                    TotalCheckedInAppointmentVaccinations = totalCheckedInAppointmentVaccinations,
                    TotalProcessedAppointmentVaccinations = totalPaidAppointmentVaccinations,
                    TotalPaidAppointmentVaccinations = totalPaidAppointmentVaccinations,
                    TotalCompletedAppointmentVaccinations = totalCompletedAppointmentVaccinations,
                    TotalCancelledAppointmentVaccinations = totalCancelledAppointmentVaccinations,
                    TotalRejectedAppointmentVaccinations = totalRejectedAppointmentVaccinations,
                    TotalAppointmentMicrochips = totalAppointmentMicrochips,
                    TotalProcessingAppointmentMicrochips = totalProcessingAppointmentMicrochips,
                    TotalConfirmedAppointmentMicrochips = totalConfirmedAppointmentMicrochips,
                    TotalCheckedInAppointmentMicrochips = totalCheckedInAppointmentMicrochips,
                    TotalProcessedAppointmentMicrochips = totalPaidAppointmentMicrochips,
                    TotalPaidAppointmentMicrochips = totalPaidAppointmentMicrochips,
                    TotalCompletedAppointmentMicrochips = totalCompletedAppointmentMicrochips,
                    TotalCancelledAppointmentMicrochips = totalCancelledAppointmentMicrochips,
                    TotalRejectedAppointmentMicrochips = totalRejectedAppointmentMicrochips,
                    TotalAppointmentHealthConditions = totalAppointmentHealthConditions,
                    TotalProcessingAppointmentHealthConditions = totalProcessingAppointmentHealthConditions,
                    TotalConfirmedAppointmentHealthConditions = totalConfirmedAppointmentHealthConditions,
                    TotalCheckedInAppointmentHealthConditions = totalCheckedInAppointmentHealthConditions,
                    TotalProcessedAppointmentHealthConditions = totalPaidAppointmentHealthConditions,
                    TotalPaidAppointmentHealthConditions = totalPaidAppointmentHealthConditions,
                    TotalCompletedAppointmentHealthConditions = totalCompletedAppointmentHealthConditions,
                    TotalCancelledAppointmentHealthConditions = totalCancelledAppointmentHealthConditions,
                    TotalRejectedAppointmentHealthConditions = totalRejectedAppointmentHealthConditions,
                    LastUpdated = DateTimeHelper.Now() 
                };
                return new BaseResponse<AdminDashboardResponseDTO>
                {
                    Code = 200,
                    Data = dashboardData,
                    Success = true,
                    Message = "Lấy dữ liệu dashboard cho admin thành công."
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving dashboard data for admin.");
                return new BaseResponse<AdminDashboardResponseDTO>
                {
                    Code = 500,
                    Success = false,
                    Message = "Đã có lỗi khi lấy dữ liệu dashboard cho admin.",
                    Data = null
                };
            }
        }

        public async Task<BaseResponse<VetDashboardResponseDTO>> GetDashboardDataForVetAsync(CancellationToken cancellationToken)
        {
            try
            {
                var totalVaccines = await _vaccineRepository.GetTotalVaccinesAsync(cancellationToken);
                var totalVaccineBatches = await _vaccineBatchRepository.GetTotalVaccineBatchesAsync(cancellationToken);
                var totalCheckedInAppointmentVaccinations = await _appointmentRepository.GetTotalCheckedInAppointmentVaccinations(cancellationToken);
                var totalCheckedinAppointmentMicrochips = await _appointmentRepository.GetTotalCheckedInAppointmentMicrochips(cancellationToken);
                var totalCheckedInAppointmentHealthConditions = await _appointmentRepository.GetTotalCheckedInAppointmentHealthConditions(cancellationToken);
                var totalProcessingAppointmentVaccinations = await _appointmentRepository.GetTotalProcessingAppointmentVaccinations(cancellationToken);
                var totalProcessingAppointmentMicrochips = await _appointmentRepository.GetTotalProcessingAppointmentMicrochips(cancellationToken);
                var totalProcessingAppointmentHealthConditions = await _appointmentRepository.GetTotalProcessingAppointmentHealthConditions(cancellationToken);
                var dashboardData = new VetDashboardResponseDTO
                {
                    TotalVaccines = totalVaccines,
                    TotalVaccineBatches = totalVaccineBatches,
                    TotalCheckedInAppointmentVaccinations = totalCheckedInAppointmentVaccinations,
                    TotalCheckedInAppointmentMicrochips = totalCheckedinAppointmentMicrochips,
                    TotalCheckedInAppointmentHealthConditions = totalCheckedInAppointmentHealthConditions,
                    TotalProcessedAppointmentVaccinations = totalProcessingAppointmentVaccinations,
                    TotalProcessedAppointmentMicrochips = totalProcessingAppointmentMicrochips,
                    TotalProcessedAppointmentHealthConditions = totalProcessingAppointmentHealthConditions,
                    LastUpdated = DateTimeHelper.Now()
                };
                return new BaseResponse<VetDashboardResponseDTO>
                {
                    Code = 200,
                    Data = dashboardData,
                    Success = true,
                    Message = "Lấy dữ liệu dashboard cho vet thành công."
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving dashboard data for vet.");
                return new BaseResponse<VetDashboardResponseDTO>
                {
                    Code = 500,
                    Success = false,
                    Message = "Đã có lỗi khi lấy dữ liệu dashboard cho vet.",
                    Data = null
                };
            }
        }
    }
}
