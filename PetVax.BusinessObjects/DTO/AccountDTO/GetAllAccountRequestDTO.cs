using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetVax.BusinessObjects.DTO.AccountDTO
{
    public class GetAllAccountRequestDTO
    {
        public int PageNumber { get; set; } = 1; // Default to the first page
        public int PageSize { get; set; } = 10; // Default to 10 items per page
        public string? KeyWord { get; set; } // Search keyword
        public bool? Status { get; set; } // Filter by status
    }
}
