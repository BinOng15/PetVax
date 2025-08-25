using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetVax.BusinessObjects.DTO.PetDTO
{
    public class CreatePetRequestDTO
    {
        [Required(ErrorMessage = "AccountId is required.")]
        public int CustomerId { get; set; }
        [Required(ErrorMessage = "Name is required.")]
        [StringLength(20, ErrorMessage = "Name cannot be longer than 20 characters.")]
        public string Name { get; set; }
        [Required(ErrorMessage = "Species is required.")]
        public string Species { get; set; } // e.g., Dog, Cat
        [Required(ErrorMessage = "Breed is required.")]
        public string Breed { get; set; }
        [Required(ErrorMessage = "Age is required.")]
        public string Gender { get; set; }
        [Required(ErrorMessage = "Date of Birth is required.")]
        [RegularExpression(@"^(0[1-9]|1[0-2])/(0[1-9]|[12][0-9]|3[01])/\d{4}$", ErrorMessage = "Date of Birth must be in mm/dd/yyyy format.")]
        public string DateOfBirth { get; set; }
        [Required(ErrorMessage = "PlaceToLive is required.")]
        public string PlaceToLive { get; set; }
        [Required(ErrorMessage = "Place of Birth is required.")]
        public string PlaceOfBirth { get; set; }
        public IFormFile? Image { get; set; }
        [Required(ErrorMessage = "Weight is required.")]
        public string Weight { get; set; } // e.g., "10 kg", "5 lbs"
        [Required(ErrorMessage = "Color is required.")]
        public string Color { get; set; } // e.g., "Brown", "Black"
        [Required(ErrorMessage = "Nationality is required.")]
        public string Nationality { get; set; } // e.g., "American", "British"
        [Required(ErrorMessage = "IsSterilized is required.")]
        public bool isSterilized { get; set; } // true if sterilized, false otherwise
    }
}
