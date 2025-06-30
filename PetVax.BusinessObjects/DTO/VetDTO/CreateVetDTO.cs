using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetVax.BusinessObjects.DTO.VetDTO
{
    public class CreateVetDTO
    {
        public string Name { get; set; }
        public string Specialization { get; set; }

        public IFormFile? Image { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string PhoneNumber { get; set; }
    }
}
