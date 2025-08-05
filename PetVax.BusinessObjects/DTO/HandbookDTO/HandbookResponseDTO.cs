using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetVax.BusinessObjects.DTO.HandbookDTO
{
    public class HandbookResponseDTO
    {
        public int HandbookId { get; set; }
        public string Title { get; set; }
        public string Introduction { get; set; }
        public string? Highlight { get; set; }
        public string Content { get; set; }
        public string? ImportantNote { get; set; }
        public string? ImageUrl { get; set; }
        public DateTime CreatedAt { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? ModifiedAt { get; set; }
        public string? ModifiedBy { get; set; }
        public bool? isDeleted { get; set; } = false;
    }
}
