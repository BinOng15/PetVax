using PetVax.BusinessObjects.DTO.CustomerDTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetVax.BusinessObjects.DTO.PointTransactionDTO
{
    public class PointTransactionResponseDTO
    {
        public int TransactionId { get; set; }
        public int CustomerId { get; set; }
        public string Change { get; set; }
        public string TransactionType { get; set; }
        public string Description { get; set; }
        public DateTime TransactionDate { get; set; }
        public DateTime CreatedAt { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? ModifiedAt { get; set; }
        public string? ModifiedBy { get; set; }
        public bool? isDeleted { get; set; } = false;

        public CustomerResponseDTO Customer { get; set; }
    }
}
