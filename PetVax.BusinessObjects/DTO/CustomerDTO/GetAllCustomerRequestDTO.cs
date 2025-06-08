using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetVax.BusinessObjects.DTO.CustomerDTO
{
    public class GetAllCustomerRequestDTO
    {
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 10;
        public string? KeyWord { get; set; }
        public bool? Status { get; set; }
    }
}
