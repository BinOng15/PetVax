using AutoMapper;
using PetVax.BusinessObjects.DTO.AccountDTO;
using PetVax.BusinessObjects.DTO.AppointmentDetailDTO;
using PetVax.BusinessObjects.DTO.AppointmentDTO;
using PetVax.BusinessObjects.DTO.CustomerDTO;
using PetVax.BusinessObjects.DTO.DiseaseDTO;
using PetVax.BusinessObjects.DTO.MicrochipDTO;
using PetVax.BusinessObjects.DTO.PetDTO;
using PetVax.BusinessObjects.DTO.VaccineBatchDTO;
using PetVax.BusinessObjects.DTO.VaccineDiseaseDTO;
using PetVax.BusinessObjects.DTO.VaccineDTO;
using PetVax.BusinessObjects.DTO.VetDTO;
using PetVax.BusinessObjects.DTO.VetScheduleDTO;
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
                .ForMember(dest => dest.AccountResponseDTO, opt => opt.MapFrom(src => src.Account));

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


            //Vaccine
            CreateMap<CreateVaccineDTO, Vaccine>();
            CreateMap<UpdateVaccineDTO, Vaccine>();
            CreateMap<Vaccine, VaccineResponseDTO>()
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

            //Disease
            CreateMap<CreateDiseaseDTO, Disease>();
            CreateMap<UpdateDiseaseDTO, Disease>();
            CreateMap<Disease, DiseaseResponseDTO>();

            //Pet
            CreateMap<CreatePetRequestDTO, Pet>();
            CreateMap<UpdatePetRequestDTO, Pet>();
            CreateMap<Pet, PetResponseDTO>()
                .ForMember(dest => dest.Image, opt => opt.MapFrom(src => src.Image == null ? null : src.Image))
                .ForMember(dest => dest.CustomerResponseDTO, opt => opt.MapFrom(src => src.Customer));

            //Appointment
            CreateMap<CreateAppointmentDTO, Appointment>();
            CreateMap<UpdateAppointmentDTO, Appointment>();
            CreateMap<Appointment, AppointmentResponseDTO>()
                .ForMember(dest => dest.CustomerResponseDTO, opt => opt.MapFrom(src => src.Customer.FullName))
                .ForMember(dest => dest.PetResponseDTO, opt => opt.MapFrom(src => src.Pet.Name))
                .ReverseMap();

            CreateMap<Appointment, AppointmentWithDetailResponseDTO>();
            CreateMap<CreateAppointmentVaccinationDTO, Appointment>();
            CreateMap<UpdateAppointmentVaccinationDTO, Appointment>();
            CreateMap<Appointment, AppointmentWithVaccinationResponseDTO>();

            //AppointmentDetail
            CreateMap<CreateAppointmentDetailDTO, AppointmentDetail>();
            CreateMap<UpdateAppointmentDetailDTO, AppointmentDetail>();
            CreateMap<AppointmentDetail, AppointmentDetailResponseDTO>()
                .ForMember(dest => dest.Disease, opt => opt.MapFrom(src => src.Disease.Name))
                .ForMember(dest => dest.MicrochipItem, opt => opt.MapFrom(src => src.MicrochipItem.Name))
                .ForMember(dest => dest.VaccineBatch, opt => opt.MapFrom(src => src.VaccineBatch.Vaccine))
                .ForMember(dest => dest.PetPassport, opt => opt.MapFrom(src => src.PetPassport.PassportCode))
                .ForMember(dest => dest.Vet, opt => opt.MapFrom(src => src.Vet.Name))
                .ReverseMap();

            CreateMap<AppointmentDetail, AppointmentVaccinationDetailResponseDTO>()
                .ForMember(dest => dest.Vet, opt => opt.MapFrom(src => src.Vet.Name))
                .ForMember(dest => dest.VaccineBatch, opt => opt.MapFrom(src => src.VaccineBatch.VaccineId))
                .ForMember(dest => dest.Disease, opt => opt.MapFrom(src => src.Disease.Name))
                .ForMember(dest => dest.Appointment, opt => opt.MapFrom(src => src.Appointment))
                .ReverseMap();

            //Microchip
            CreateMap<Microchip, MicrochipResponseDTO>();
            CreateMap<MicrochipRequestDTO, Microchip>();
        }
    }   
}
