﻿using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetVax.BusinessObjects.DTO.VetDTO
{
    public class UpdateVetRequest
    {
        public int VetId { get; set; }
        public string? Name { get; set; }
        public IFormFile? Image { get; set; }
        public string? Specialization { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public string? PhoneNumber { get; set; }
 
    }
}
