using AutoMapper;
using PetVax.BusinessObjects.DTO.AccountDTO;
using PetVax.BusinessObjects.DTO.AddressDTO;
using PetVax.BusinessObjects.DTO.AppointmentDetailDTO;
using PetVax.BusinessObjects.DTO.AppointmentDTO;
using PetVax.BusinessObjects.DTO.ColdChainLogDTO;
using PetVax.BusinessObjects.DTO.CustomerDTO;
using PetVax.BusinessObjects.DTO.CustomerVoucherDTO;
using PetVax.BusinessObjects.DTO.DiseaseDTO;
using PetVax.BusinessObjects.DTO.FAQItemDTO;
using PetVax.BusinessObjects.DTO.HandbookDTO;
using PetVax.BusinessObjects.DTO.HealthConditionDTO;
using PetVax.BusinessObjects.DTO.MembershipDTO;
using PetVax.BusinessObjects.DTO.MicrochipDTO;
using PetVax.BusinessObjects.DTO.MicrochipItemDTO;
using PetVax.BusinessObjects.DTO.PaymentDTO;
using PetVax.BusinessObjects.DTO.PetDTO;
using PetVax.BusinessObjects.DTO.PointTransactionDTO;
using PetVax.BusinessObjects.DTO.ServiceHistoryDTO;
using PetVax.BusinessObjects.DTO.SupportCategoryDTO;
using PetVax.BusinessObjects.DTO.VaccinationCertificate;
using PetVax.BusinessObjects.DTO.VaccinationSchedule;
using PetVax.BusinessObjects.DTO.VaccineBatchDTO;
using PetVax.BusinessObjects.DTO.VaccineDiseaseDTO;
using PetVax.BusinessObjects.DTO.VaccineDTO;
using PetVax.BusinessObjects.DTO.VaccineExportDetailDTO;
using PetVax.BusinessObjects.DTO.VaccineExportDTO;
using PetVax.BusinessObjects.DTO.VaccineProfileDTO;
using PetVax.BusinessObjects.DTO.VaccineReceipDetailDTO;
using PetVax.BusinessObjects.DTO.VaccineReceiptDTO;
using PetVax.BusinessObjects.DTO.VetDTO;
using PetVax.BusinessObjects.DTO.VetScheduleDTO;
using PetVax.BusinessObjects.DTO.VoucherDTO;
using PetVax.BusinessObjects.Enum;
using PetVax.BusinessObjects.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetVax.Services.Configurations.Mapper
{
    public class MapperEntities : Profile
    {
        public MapperEntities()
        {
            //Account
            CreateMap<CreateAccountDTO, Account>()
                .ForMember(dest => dest.PasswordHash, opt => opt.Ignore())
                .ForMember(dest => dest.PasswordSalt, opt => opt.Ignore());
            CreateMap<UpdateAccountDTO, Account>()
                .ForMember(dest => dest.PasswordHash, opt => opt.Ignore())
                .ForMember(dest => dest.PasswordSalt, opt => opt.Ignore());
            CreateMap<Account, AccountResponseDTO>();

            //Customer
            CreateMap<CreateCustomerDTO, Customer>();
            CreateMap<UpdateCustomerDTO, Customer>();
            CreateMap<Customer, CustomerResponseDTO>()
                .ForMember(dest => dest.Image, opt => opt.MapFrom(src => src.Image == null ? null : src.Image))
                .ForMember(dest => dest.AccountResponseDTO, opt => opt.MapFrom(src => src.Account))
                .ForMember(dest => dest.MembershipResponseDTO, opt => opt.MapFrom(src => src.Membership));

            //Vet
            CreateMap<Vet, VetResponseDTO>()
                .ForMember(dest => dest.Account, opt => opt.MapFrom(src => src.Account)).ReverseMap();
            CreateMap<CreateVetDTO, Vet>();
            CreateMap<UpdateVetRequest, Vet>();

            //VetSchedule
            CreateMap<CreateVetScheduleRequestDTO, VetSchedule>();
            CreateMap<UpdateVetScheduleRequestDTO, VetSchedule>();
            CreateMap<VetSchedule, VetScheduleDTO>()
                .ForMember(dest => dest.VetResponse, opt => opt.MapFrom(src => src.Vet)).ReverseMap();
            CreateMap<VetSchedule, ScheduleResponse>();
            CreateMap<Vet, VetResponseDTO>()
                .ForMember(dest => dest.ScheduleResponse, opt => opt.MapFrom(src => src.VetSchedules));

            //Vaccine
            CreateMap<CreateVaccineDTO, Vaccine>();
            CreateMap<UpdateVaccineDTO, Vaccine>();
            CreateMap<Vaccine, VaccineResponseDTO>()
                .ForMember(dest => dest.Image, opt => opt.MapFrom(src => src.Image == null ? null : src.Image));

            CreateMap<Vaccine, VaccineForBatchVaccineProfileResponseDTO>()
                .ForMember(dest => dest.Image, opt => opt.MapFrom(src => src.Image == null ? null : src.Image));

            //VaccineDisease
            CreateMap<CreateVaccineDiseaseDTO, VaccineDisease>();
            CreateMap<UpdateVaccineDiseaseDTO, VaccineDisease>();
            CreateMap<VaccineDisease, VaccineDiseaseResponseDTO>()
                .ForMember(dest => dest.VaccineResponseDTO, opt => opt.MapFrom(src => src.Vaccine))
                .ForMember(dest => dest.DiseaseResponseDTO, opt => opt.MapFrom(src => src.Disease))
                .ReverseMap();

            // VaccineBatch
            CreateMap<CreateVaccineBatchDTO, VaccineBatch>();
            CreateMap<UpdateVaccineBatchDTO, VaccineBatch>();
            CreateMap<VaccineBatch, VaccineBatchResponseDTO>()
                .ForMember(dest => dest.VaccineResponseDTO, opt => opt.MapFrom(src => src.Vaccine))
                .ReverseMap();

            CreateMap<VaccineBatch, VaccineBatchVaccineProfileResponseDTO>()
                .ForMember(dest => dest.Vaccine, opt => opt.MapFrom(src => src.Vaccine));

            //Disease
            CreateMap<CreateDiseaseDTO, Disease>();
            CreateMap<UpdateDiseaseDTO, Disease>();
            CreateMap<Disease, DiseaseResponseDTO>();
            CreateMap<Disease, DiseaseForVaccinationScheduleResponseDTO>();

            //Pet
            CreateMap<CreatePetRequestDTO, Pet>();
            CreateMap<UpdatePetRequestDTO, Pet>();
            CreateMap<Pet, PetResponseDTO>()
                .ForMember(dest => dest.Image, opt => opt.MapFrom(src => src.Image == null ? null : src.Image))
                .ForMember(dest => dest.CustomerResponseDTO, opt => opt.MapFrom(src => src.Customer));
            CreateMap<Pet, PetResponseDTOs>();
            CreateMap<Pet, PetHasMicrochipResponseDTO>()
                .ForMember(dest => dest.Image, opt => opt.MapFrom(src => src.Image == null ? null : src.Image))
                .ForMember(dest => dest.CustomerResponseDTO, opt => opt.MapFrom(src => src.Customer))
                .ForMember(dest => dest.MicrochipItems, otp => otp.MapFrom(src => src.MicrochipItems));

            //Appointment
            CreateMap<CreateAppointmentDTO, Appointment>();
            CreateMap<UpdateAppointmentDTO, Appointment>();
            CreateMap<Appointment, AppointmentResponseDTO>()
                .ForMember(dest => dest.CustomerResponseDTO, opt => opt.MapFrom(src => src.Customer))
                .ForMember(dest => dest.PetResponseDTO, opt => opt.MapFrom(src => src.Pet));

            CreateMap<Appointment, AppointmentWithDetailResponseDTO>();
            CreateMap<CreateAppointmentVaccinationDTO, Appointment>();
            CreateMap<UpdateAppointmentForVaccinationDTO, AppointmentForVaccinationResponseDTO>();
            CreateMap<Appointment, AppointmentWithVaccinationResponseDTO>();

            CreateMap<CreateAppointmentVaccinationCertificateDTO, AppointmentWithVaccinationCertificateResponseDTO>();
            CreateMap<UpdateAppointmentVaccinationCertificateDTO, AppointmentVaccinationCertificateResponseDTO>();
            CreateMap<Appointment, AppointmentWithVaccinationCertificateResponseDTO>();
            CreateMap<Appointment, AppointmentDTO>();

            //AppointmentDetail
            CreateMap<CreateAppointmentDetailDTO, AppointmentDetail>();
            CreateMap<UpdateAppointmentDetailDTO, AppointmentDetail>();
            CreateMap<UpdateAppointmentVaccinationDTO, Appointment>();
            CreateMap<AppointmentDetail, AppointmentDetailResponseDTO>()
                .ForMember(dest => dest.Vet, opt => opt.MapFrom(src => src.Vet))
                .ForMember(dest => dest.MicrochipItem, opt => opt.MapFrom(src => src.MicrochipItem))
                .ForMember(dest => dest.PetPassport, opt => opt.MapFrom(src => src.VaccinationCertificate))
                .ForMember(dest => dest.HealthCondition, opt => opt.MapFrom(src => src.HealthCondition))
                .ForMember(dest => dest.VaccineBatch, opt => opt.MapFrom(src => src.VaccineBatch))
                .ForMember(dest => dest.Disease, opt => opt.MapFrom(src => src.Disease));

            CreateMap<AppointmentDetail, AppointmentForVaccinationResponseDTO>();
            CreateMap<AppointmentDetail, AppointmentHasDiseaseResponseDTO>()
                .ForMember(dest => dest.Disease, opt => opt.MapFrom(src => src.Disease));
            CreateMap<AppointmentDetail, AppointmentVaccinationDetailResponseDTO>()
                .ForMember(dest => dest.Vet, opt => opt.MapFrom(src => src.Vet))
                .ForMember(dest => dest.VaccineBatch, opt => opt.MapFrom(src => src.VaccineBatch))
                .ForMember(dest => dest.Disease, opt => opt.MapFrom(src => src.Disease))
                .ForMember(dest => dest.Appointment, opt => opt.MapFrom(src => src.Appointment))
                .ForMember(dest => dest.Payment, opt => opt.MapFrom(src => src.Payment));

            CreateMap<AppointmentDetail, AppointmentMicrochipResponseDTO>()
                .ForMember(dest => dest.Vet, opt => opt.MapFrom(src => src.Vet))
                .ForMember(dest => dest.MicrochipItem, opt => opt.MapFrom(src => src.MicrochipItem))
                .ForMember(dest => dest.Appointment, opt => opt.MapFrom(src => src.Appointment))
                .ForMember(dest => dest.Payment, opt => opt.MapFrom(src => src.Payment));
            CreateMap<AppointmentDetail, AppointmenDetialMicorchipResponseDTO>()
                .ForMember(dest => dest.Microchip, opt => opt.MapFrom(src => src));

            CreateMap<AppointmentDetail, AppointmentVaccinationCertificateResponseDTO>()
                .ForMember(dest => dest.Vet, opt => opt.MapFrom(src => src.Vet))
                .ForMember(dest => dest.Disease, opt => opt.MapFrom(src => src.Disease))
                .ForMember(dest => dest.VaccinationCertificate, opt => opt.MapFrom(src => src.VaccinationCertificate));

            CreateMap<AppointmentDetail, AppointmentVaccinationForProfileResponseDTO>()
                .ForMember(dest => dest.Vet, opt => opt.MapFrom(src => src.Vet))
                .ForMember(dest => dest.VaccineBatch, opt => opt.MapFrom(src => src.VaccineBatch));

            //Microchip
            CreateMap<Microchip, MicrochipResponseDTO>();
            CreateMap<MicrochipRequestDTO, Microchip>();
            CreateMap<MicrochipItem, MicrochipItemResponseDTO>();
            CreateMap<CreateMicrochipItemRequest, MicrochipItem>();
            CreateMap<MicrochipItem, BaseMicrochipItemResponse>()
                .ForMember(dest => dest.MicrochipResponse, opt => opt.MapFrom(src => src.Microchip));
            CreateMap<UpdateMicrochipItemRequest, MicrochipItem>();

            CreateMap<Pet, PetMicrochipItemResponse>()
             .ForMember(dest => dest.AppointmentDetails, opt => opt.Ignore())
             .ForMember(dest => dest.Customer, opt => opt.Ignore());
            CreateMap<MicrochipItem, MicrochipItemResponse>()
                .ForMember(dest => dest.Pet, opt => opt.Ignore());
            CreateMap<HealthCondition, HealthConditionResponse>();


            //VaccineProfile
            CreateMap<VaccineProfile, VaccineProfileResponseDTO>()
                //.ForMember(dest => dest.AppointmentDetail, opt => opt.MapFrom(src => src.AppointmentDetail))
                .ForMember(dest => dest.AppointmentDetail, opt => opt.Ignore());

            //VaccinationCertificate
            CreateMap<CreateVaccinationCertificateDTO, VaccinationCertificate>();
            CreateMap<UpdateVaccinationCertificateDTO, VaccinationCertificate>();
            CreateMap<VaccinationCertificate, VaccinationCertificateResponseDTO>()
                .ForMember(dest => dest.Pet, opt => opt.MapFrom(src => src.Pet))
                .ForMember(dest => dest.Vet, opt => opt.MapFrom(src => src.Vet))
                .ForMember(dest => dest.Disease, opt => opt.MapFrom(src => src.Disease))
                .ForMember(dest => dest.MicrochipItem, opt => opt.MapFrom(src => src.MicrochipItem));

            //Payment
            CreateMap<CreatePaymentRequestDTO, Payment>();
            CreateMap<UpdatePaymentRequestDTO, Payment>();
            CreateMap<Payment, PaymentResponseDTO>();
            CreateMap<Payment, PaymentForTransactionResponseDTO>();

            //HealthCondition
            CreateMap<CreateHealthConditionDTO, HealthCondition>();
            CreateMap<UpdateHealthCondition, HealthCondition>();
            CreateMap<HealthCondition, BaseHealthConditionResponseDTO>()
                .ForMember(dest => dest.Pet, opt => opt.MapFrom(src => src.Pet))
                .ForMember(dest => dest.Vet, opt => opt.MapFrom(src => src.Vet));
            CreateMap<HealthCondition, HealthConditionResponse>();
            CreateMap<AppointmentDetail, AppointmentHealthConditionResponseDTO>()
                .ForMember(dest => dest.HealthCondition, opt => opt.MapFrom(src => src.HealthCondition))
                .ForMember(dest => dest.Vet, opt => opt.MapFrom(src => src.Vet))
                .ForMember(dest => dest.Appointment, opt => opt.MapFrom(src => src.Appointment))
                .ForMember(dest => dest.Payment, opt => opt.MapFrom(src => src.Payment));
            CreateMap<AppointmentDetail, AppointmentDetailHealthConditionResponseDTO>();
            CreateMap<UpdateAppointmentHealthConditionDTO, HealthCondition>();
            //VaccinationSchedule
            CreateMap<CreateVaccinationScheduleDTO, VaccinationSchedule>();
            CreateMap<UpdateVaccinationScheduleDTO, VaccinationSchedule>();
            CreateMap<VaccinationSchedule, VaccinationScheduleResponseDTO>()
                .ForMember(dest => dest.Disease, opt => opt.MapFrom(src => src.Disease));

            //VaccineReceipt
            CreateMap<CreateVaccineReceiptDTO, VaccineReceipt>();
            CreateMap<CreateFullVaccineReceiptDTO, VaccineReceipt>();
            CreateMap<UpdateVaccineReceiptDTO, VaccineReceipt>();
            CreateMap<VaccineReceipt, VaccineReceiptResponseDTO>();

            //VaccineReceiptDetail
            CreateMap<CreateVaccineReceiptDetailDTO, VaccineReceiptDetail>();
            CreateMap<UpdateVaccineReceiptDetailDTO, VaccineReceiptDetail>();
            CreateMap<VaccineReceiptDetail, VaccineReceiptDetailResponseDTO>()
                .ForMember(dest => dest.VaccineBatch, opt => opt.MapFrom(src => src.VaccineBatch))
                .ForMember(dest => dest.VaccineReceipt, opt => opt.MapFrom(src => src.VaccineReceipt));
            CreateMap<CreateFullVaccineReceiptDTO, VaccineReceiptDetail>();


            //ColdChainLog
            CreateMap<CreateColdChainLogDTO, ColdChainLog>();
            CreateMap<UpdateColdChainLogDTO, ColdChainLog>();
            CreateMap<ColdChainLog, ColdChainLogResponseDTO>()
                .ForMember(dest => dest.VaccineBatch, opt => opt.MapFrom(src => src.VaccineBatch));

            //VaccineExport
            CreateMap<CreateVaccineExportDTO, VaccineExport>();
            CreateMap<CreateFullVaccineExportDTO, VaccineExport>();
            CreateMap<UpdateVaccineExportDTO, VaccineExport>();
            CreateMap<VaccineExport, VaccineExportResponseDTO>();

            //VaccineExportDetail
            CreateMap<CreateVaccineExportDetailDTO, VaccineExportDetail>();
            CreateMap<CreateFullVaccineExportDTO, VaccineExportDetail>();
            CreateMap<UpdateVaccineExportDetailDTO, VaccineExportDetail>();
            CreateMap<UpdateVaccineExportDetailForVaccinationDTO, VaccineExportDetail>();
            CreateMap<VaccineExportDetail, VaccineExportDetailResponseDTO>()
                .ForMember(dest => dest.VaccineBatch, opt => opt.MapFrom(src => src.VaccineBatch))
                .ForMember(dest => dest.VaccineExport, opt => opt.MapFrom(src => src.VaccineExport));
            CreateMap<VaccineExportDetail, VaccineExportDetailResponseForVaccinationDTO>()
                .ForMember(dest => dest.VaccineBatch, opt => opt.MapFrom(src => src.VaccineBatch))
                .ForMember(dest => dest.VaccineExport, opt => opt.MapFrom(src => src.VaccineExport))
                .ForMember(dest => dest.AppointmentDetailId, opt => opt.MapFrom(src => src.AppointmentDetailId));

            //Membership
            CreateMap<CreateMembershipDTO, Membership>();
            CreateMap<UpdateMembershipDTO, Membership>();
            CreateMap<Membership, MembershipResponseDTO>()
                .ForMember(dest => dest.Customer, opt => opt.MapFrom(src => src.Customers));
            CreateMap<Membership, CreateUpdateMembershipResponseDTO>();
            CreateMap<Membership, MembershipStatusResponseDTO>();

            //Voucher
            CreateMap<CreateVoucherDTO, Voucher>();
            CreateMap<UpdateVoucherDTO, Voucher>();
            CreateMap<Voucher, VoucherResponseDTO>()
                .ForMember(dest => dest.PointTransaction, opt => opt.MapFrom(src => src.PointTransaction));
            CreateMap<Voucher, CreateUpdateVoucherResponseDTO>();
            CreateMap<Voucher, CustomerVoucherResponseDTO>();
            CreateMap<Voucher, VoucherForTransactionResponseDTO>();

            //PointTransaction
            CreateMap<PointTransaction, PointTransactionResponseDTO>()
                .ForMember(dest => dest.Customer, opt => opt.MapFrom(src => src.Customer))
                .ForMember(dest => dest.Payment, opt => opt.MapFrom(src => src.Payment))
                .ForMember(dest => dest.Voucher, opt => opt.MapFrom(src => src.Voucher));

            //CustomerVoucher
            CreateMap<CustomerVoucher, CustomerVoucherResponseDTO>()
                .ForMember(dest => dest.Customer, opt => opt.MapFrom(src => src.Customer))
                .ForMember(dest => dest.Voucher, opt => opt.MapFrom(src => src.Voucher));
            //ServiceHistory
            CreateMap<ServiceHistory, ServiceHistoryResponseDTO>()
                .ForMember(dest => dest.Customer, opt => opt.MapFrom(src => src.Customer))
                .ForMember(dest => dest.ServiceType, opt => opt.MapFrom(src => Convert.ToInt32(src.ServiceType)))
                .ForMember(dest => dest.Pet, opt => opt.MapFrom(src => src.Pet))
                .ForMember(dest => dest.Appointment, opt => opt.MapFrom(src => src.Appointment));

            //Handbook
            CreateMap<CreateHandbookDTO, Handbook>();
            CreateMap<UpdateHandbookDTO, Handbook>();
            CreateMap<Handbook, HandbookResponseDTO>()
                .ForMember(dest => dest.ImageUrl, opt => opt.MapFrom(src => src.ImageUrl == null ? null : src.ImageUrl));

            //SupportCategory
            CreateMap<CreateSupportCategoryDTO, SupportCategory>();
            CreateMap<UpdateSupportCategoryDTO, SupportCategory>();
            CreateMap<SupportCategory, SupportCategoryResponseDTO>();

            //FAQItem
            CreateMap<CreateFAQDTO, FAQItem>();
            CreateMap<UpdateFAQDTO, FAQItem>();
            CreateMap<FAQItem, FAQResponseDTO>();

            //Address
            CreateMap<Address, AddressResponseDTO>();

        }
    }   
}


