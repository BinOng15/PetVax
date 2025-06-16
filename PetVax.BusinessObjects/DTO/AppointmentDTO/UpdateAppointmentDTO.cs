using System;
using System.Collections.Generic;
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
        public string? ServiceType { get; set; }
        public string? Location { get; set; }
        public string? Address { get; set; }
    }
}
