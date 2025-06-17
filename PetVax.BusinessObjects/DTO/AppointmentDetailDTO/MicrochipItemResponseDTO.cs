namespace PetVax.BusinessObjects.DTO.AppointmentDetailDTO
{
    //Đây là giả định về DTO cho MicrochipItemResponseDTO
    public class MicrochipItemResponseDTO
    {
        public int MicrochipItemId { get; set; } // Unique identifier for the microchip item
        public string MicrochipCode { get; set; } // e.g., "MICRO123456", unique identifier for the microchip
        public string? Manufacturer { get; set; } // Manufacturer of the microchip
        public string? DateOfManufacture { get; set; } // Date when the microchip was manufactured
        public string? DateOfImplantation { get; set; } // Date when the microchip was implanted in the pet
        public string? ImplantationLocation { get; set; } // Location on the pet's body where the microchip was implanted
        public string? Status { get; set; } // Status of the microchip (e.g., Active, Inactive)
        public string? CreatedAt { get; set; } // Creation date of the microchip item record
        public string? CreatedBy { get; set; } // User who created the microchip item record
        public string? ModifiedAt { get; set; } // Last modification date of the microchip item record
        public string? ModifiedBy { get; set; } // User who last modified the microchip item record
    }
}