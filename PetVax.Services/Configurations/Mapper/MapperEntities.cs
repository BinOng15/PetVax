using AutoMapper;
using PetVax.BusinessObjects.DTO.AccountDTO;
using PetVax.BusinessObjects.DTO.AppointmentDTO;
using PetVax.BusinessObjects.DTO.CustomerDTO;
using PetVax.BusinessObjects.DTO.DiseaseDTO;
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
            CreateMap<Customer, CustomerResponseDTO>();

            //Vet
            CreateMap<Vet, VetResponseDTO>();

            //VetSchedule
            CreateMap<CreateVetScheduleRequestDTO, VetSchedule>();
            CreateMap<UpdateVetScheduleRequestDTO, VetSchedule>();
            CreateMap<VetSchedule, VetScheduleDTO>();

            //Vaccine
            CreateMap<CreateVaccineDTO, Vaccine>();
            CreateMap<UpdateVaccineDTO, Vaccine>();
            CreateMap<Vaccine, VaccineResponseDTO>()
                .ForMember(dest => dest.Image, opt => opt.MapFrom(src => src.Image == null ? null : src.Image));

            //VaccineDisease
            CreateMap<CreateVaccineDiseaseDTO, VaccineDisease>();
            CreateMap<UpdateVaccineDiseaseDTO, VaccineDisease>();
            CreateMap<VaccineDisease, VaccineDiseaseResponseDTO>();

            // VaccineBatch
            CreateMap<CreateVaccineBatchDTO, VaccineBatch>();
            CreateMap<UpdateVaccineBatchDTO, VaccineBatch>();
            CreateMap<VaccineBatch, VaccineBatchResponseDTO>();

            //Disease
            CreateMap<CreateDiseaseDTO, Disease>();
            CreateMap<UpdateDiseaseDTO, Disease>();
            CreateMap<Disease, DiseaseResponseDTO>();

            //Pet
            CreateMap<CreatePetRequestDTO, Pet>();
            CreateMap<UpdatePetRequestDTO, Pet>();
            CreateMap<Pet, PetResponseDTO>()
                .ForMember(dest => dest.Image, opt => opt.MapFrom(src => src.Image == null ? null : src.Image));

            //Appointment
            CreateMap<CreateAppointmentDTO, Appointment>();
            CreateMap<UpdateAppointmentDTO, Appointment>();
            CreateMap<Appointment, AppointmentResponseDTO>();

        }
    }   
}
