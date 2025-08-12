using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetVax.BusinessObjects.DTO.AddressDTO
{
    public class CreateAddressDTO
    {
        [Required(ErrorMessage = "Địa chỉ không được để trống")]
        [StringLength(500, ErrorMessage = "Địa chỉ không được vượt quá 500 ký tự")]
        public string Location { get; set; }
    }
}
