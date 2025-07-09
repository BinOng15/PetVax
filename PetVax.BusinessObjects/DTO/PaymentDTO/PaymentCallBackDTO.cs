using PetVax.BusinessObjects.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetVax.BusinessObjects.DTO.PaymentDTO
{
    public class PaymentCallBackDTO
    {
        public EnumList.PaymentStatus PaymentStatus { get; set; }
        public int PaymentId { get; set; }

    }
}
