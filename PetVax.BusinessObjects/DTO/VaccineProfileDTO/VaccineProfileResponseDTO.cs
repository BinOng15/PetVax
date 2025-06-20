﻿using PetVax.BusinessObjects.DTO.DiseaseDTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetVax.BusinessObjects.DTO.VaccineProfileDTO
{
    public class VaccineProfileResponseDTO
    {
        public int VaccineProfileId { get; set; } 
        public int PetId { get; set; } 

        public DateTime? PreferedDate { get; set; } 
        public DateTime? VaccinationDate { get; set; }
        public string? Dose { get; set; } 
        public string? Reaction { get; set; } 
        public string? NextVaccinationInfo { get; set; }
        public bool? IsActive { get; set; } 
        public bool? IsCompleted { get; set; } 
        public DateTime CreatedAt { get; set; } 

        public DiseaseResponseDTO Disease { get; set; }
    }
}
