using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetVax.BusinessObjects.DTO.FAQItemDTO
{
    public class UpdateFAQDTO
    {
        [StringLength(500, ErrorMessage = "Câu hỏi không được vượt quá 500 ký tự.")]
        public string? Question { get; set; }
        [StringLength(2000, ErrorMessage = "Câu trả lời không được vượt quá 2000 ký tự.")]
        public string? Answer { get; set; }
    }
}
