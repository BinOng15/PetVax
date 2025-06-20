﻿using PetVax.BusinessObjects.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetVax.BusinessObjects.DTO.AppointmentDetailDTO
{
    public class UpdateAppointmentDetailDTO
    {
        public int? AppointmentId { get; set; }
        public int? VetId { get; set; }
        public int? DiseaseId { get; set; }
        public int? VaccineBatchId { get; set; }
        public int? MicrochipItemId { get; set; }
        public int? PassportId { get; set; }
        public int? HealthConditionId { get; set; }
        public string? Dose { get; set; }
        public string? Reaction { get; set; }
        public string? NextVaccinationInfo { get; set; }
        public DateTime? AppointmentDate { get; set; }
        public EnumList.AppointmentStatus? AppointmentStatus { get; set; }
    }
}
