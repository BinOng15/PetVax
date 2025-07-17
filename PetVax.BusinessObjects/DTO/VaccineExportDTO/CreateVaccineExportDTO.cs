using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetVax.BusinessObjects.DTO.VaccineExportDTO
{
    public class CreateVaccineExportDTO
    {
        [Required(ErrorMessage = "Vui lòng chọn ngày xuất kho.")]
        public DateTime ExportDate { get; set; }
    }
}
