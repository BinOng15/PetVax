using PetVax.BusinessObjects.DTO.AppointmentDetailDTO;
using PetVax.BusinessObjects.Enum;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetVax.BusinessObjects.DTO.AppointmentDTO
{
    public class UpdateAppointmentDTO
    {
        public int? CustomerId { get; set; }
        public int? PetId { get; set; }
        public DateTime? AppointmentDate { get; set; }
        public EnumList.ServiceType? ServiceType { get; set; }
        public EnumList.Location? Location { get; set; }
        public string? Address { get; set; }
    }
    public class UpdateAppointmentForVaccinationDTO
    {
        public UpdateAppointmentDTO Appointment { get; set; }
        public UpdateDiseaseForAppointmentDTO UpdateDiseaseForAppointmentDTO { get; set; }
    }
    public class UpdateDiseaseForAppointmentDTO
    {
        public int? DiseaseId { get; set; }
    }
}
