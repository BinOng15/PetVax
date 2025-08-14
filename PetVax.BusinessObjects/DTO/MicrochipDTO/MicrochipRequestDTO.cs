using PetVax.BusinessObjects.DTO.MicrochipItemDTO;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetVax.BusinessObjects.DTO.MicrochipDTO
{
    public class MicrochipRequestDTO
    {
        [Required(ErrorMessage = "Microchipcode không được để trống!")]
        public string MicrochipCode { get; set; }
        [Required(ErrorMessage = "Tên không được để trống!")]
        public string Name { get; set; }

        public string? Description { get; set; }
        [Required(ErrorMessage = "Giá cả không được để trống!")]
        public decimal Price { get; set; }
        public string? Notes { get; set; }
        public CreateMicrochipItemRequest? createMicrochipItemRequest { get; set; } 
    }
}
