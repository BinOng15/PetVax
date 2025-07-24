using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetVax.BusinessObjects.DTO.VaccineReceiptDTO
{
    public class CreateVaccineReceiptDTO
    {
        [Required(ErrorMessage = "Vui lòng chọn ngày nhập kho.")]
        public DateTime ReceiptDate { get; set; }
    }
}
