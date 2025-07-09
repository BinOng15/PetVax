using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetVax.BusinessObjects.DTO.HealthConditionDTO
{
    public class HealthConditionResponse
    {
        public int HealthConditionId { get; set; }
        public int PetId { get; set; }
        public int VetId { get; set; }
        public int MicrochipItemId { get; set; }
        public string ConditionCode { get; set; }
        public string HeartRate { get; set; }
        public string BreathingRate { get; set; }
        public string Weight { get; set; }
        public string Temperature { get; set; } // "38.5 °C", "101.5 °F"
        public string EHNM { get; set; } //Mắt tai mũi họng
        public string SkinAFur { get; set; } //Da và lông
        public string Digestion { get; set; } //Tiêu hóa
        public string Respiratory { get; set; } //Hô hấp
        public string Excrete { get; set; } //Bài tiết
        public string Behavior { get; set; } //Hành vi
        public string Psycho { get; set; } //Tâm lý
        public string Different { get; set; } //Những điều khác
        public string Conclusion { get; set; } // Kết luận
        public decimal Price { get; set; }
        public DateTime CheckDate { get; set; }
        public string Status { get; set; }
        public DateTime CreatedAt { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? ModifiedAt { get; set; }
        public string? ModifiedBy { get; set; }
        public bool? isDeleted { get; set; } = false;
    }
}
