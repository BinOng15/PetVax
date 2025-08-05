using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetVax.BusinessObjects.DTO.SupportCategoryDTO
{
    public class CreateSupportCategoryDTO
    {
        [Required(ErrorMessage = "Vui lòng nhập tiêu đề.")]
        [StringLength(100, ErrorMessage = "Tiêu đề không được vượt quá 100 ký tự.")]
        public string Title { get; set; }
        [Required(ErrorMessage = "Vui lòng nhập mô tả.")]
        [StringLength(500, ErrorMessage = "Mô tả không được vượt quá 500 ký tự.")]
        public string Description { get; set; }
        [Required(ErrorMessage = "Vui lòng nhập nội dung.")]
        [StringLength(2000, ErrorMessage = "Nội dung không được vượt quá 2000 ký tự.")]
        public string Content { get; set; }
    }
}
