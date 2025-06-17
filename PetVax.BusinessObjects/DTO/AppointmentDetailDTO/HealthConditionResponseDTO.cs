namespace PetVax.BusinessObjects.DTO.AppointmentDetailDTO
{
    //Đây là giả định về DTO cho phản hồi của điều kiện sức khỏe trong chi tiết cuộc hẹn
    public class HealthConditionResponseDTO
    {
        public int HealthConditionId { get; set; } // Unique identifier for the health condition
        public string Name { get; set; } // Name of the health condition
        public string Description { get; set; } // Description of the health condition
        public DateTime CreatedAt { get; set; } // Date and time when the health condition was created
        public string CreatedBy { get; set; } // User who created the health condition
        public DateTime? ModifiedAt { get; set; } // Date and time when the health condition was last modified
        public string? ModifiedBy { get; set; } // User who last modified the health condition
    }
}