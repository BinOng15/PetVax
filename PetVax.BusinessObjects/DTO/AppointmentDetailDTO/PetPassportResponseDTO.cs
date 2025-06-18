namespace PetVax.BusinessObjects.DTO.AppointmentDetailDTO
{
    //Đây là giả định về DTO cho phản hồi của hộ chiếu thú cưng.
    public class PetPassportResponseDTO
    {
        public int PetPassportId { get; set; }
        public int PetId { get; set; }
        public string PassportCode { get; set; } // e.g., "PP123456", unique identifier for the pet passport
        public string PetName { get; set; } // Name of the pet
        public string Species { get; set; } // e.g., Dog, Cat
        public string Breed { get; set; } // Breed of the pet
        public string DateOfBirth { get; set; } // Date of birth of the pet in "yyyy-MM-dd" format
        public string PlaceOfBirth { get; set; } // Place of birth of the pet
        public string PlaceToLive { get; set; } // Place where the pet currently lives
    }
}