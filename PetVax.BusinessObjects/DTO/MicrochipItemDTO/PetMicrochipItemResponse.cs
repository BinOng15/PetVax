﻿using PetVax.BusinessObjects.DTO.AppointmentDetailDTO;
using PetVax.BusinessObjects.DTO.CustomerDTO;
using PetVax.BusinessObjects.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetVax.BusinessObjects.DTO.MicrochipItemDTO
{
    public class PetMicrochipItemResponse
    {
        public int PetId { get; set; }
        public int CustomerId { get; set; }
        public string PetCode { get; set; } // e.g., "PET123456", unique identifier for the pet
        public string Name { get; set; }
        public string Species { get; set; }
        public string Breed { get; set; }
        public string Gender { get; set; }
        public string DateOfBirth { get; set; }
        public string PlaceToLive { get; set; }
        public string PlaceOfBirth { get; set; }
        public string Image { get; set; }
        public string Weight { get; set; }
        public string Color { get; set; }
        public string Nationality { get; set; }
        public bool isSterilized { get; set; }
        public List<AppointmentDetailResponseDTO> AppointmentDetails { get; set; } // Details of the appointment associated with the pet
        public CustomerResponseDTO Customer { get; set; } // Details of the customer associated with the pet
    }
}
