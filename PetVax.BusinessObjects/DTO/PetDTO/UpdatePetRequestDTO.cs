using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetVax.BusinessObjects.DTO.PetDTO
{
    public class UpdatePetRequestDTO
    {
        public string? Name { get; set; }
        public string? Species { get; set; } // e.g., Dog, Cat
        public string? Breed { get; set; }
        public string? Gender { get; set; }
        public string? DateOfBirth { get; set; }
        public string? PlaceToLive { get; set; }
        public string? PlaceOfBirth { get; set; }
        public IFormFile? Image { get; set; }
        public string? Weight { get; set; } // e.g., "10 kg", "5 lbs"
        public string? Color { get; set; } // e.g., "Brown", "Black"
        public string? Nationality { get; set; } // e.g., "American", "British"
        public bool? isSterilized { get; set; } // true if sterilized, false otherwise
    }
}
