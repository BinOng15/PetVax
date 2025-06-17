using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetVax.BusinessObjects.DTO.VaccineDiseaseDTO
{
    public class VaccineDiseaseResponseDTO
    {
        public int VaccineDiseaseId { get; set; } // Unique identifier for the vaccine disease record
        public int VaccineId { get; set; } // Foreign key to Vaccine table
        public int DiseaseId { get; set; } // Foreign key to Disease table
        public DateTime CreatedAt { get; set; } // Date when the record was created
        public string CreatedBy { get; set; } // User who created the record
        public DateTime? ModifiedAt { get; set; } // Date when the record was last modified
        public string? ModifiedBy { get; set; } // User who last modified the record
    }
}
