using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetVax.BusinessObjects.DTO.HandbookDTO
{
    public class CreateHandbookDTO
    {
        [Required(ErrorMessage = "Vui lòng nhập tiêu đề.")]
        [StringLength(100, ErrorMessage = "Tiêu đề không được vượt quá 100 ký tự.")]
        public string Title { get; set; }
        [Required(ErrorMessage = "Vui lòng nhập phần giới thiệu.")]
        [StringLength(500, ErrorMessage = "Phần giới thiệu không được vượt quá 500 ký tự.")]
        public string Introduction { get; set; }
        [StringLength(500, ErrorMessage = "Nội dung nổi bật không được vượt quá 500 ký tự.")]
        public string? Highlight { get; set; }
        [Required(ErrorMessage = "Vui lòng nhập nội dung.")]
        public string Content { get; set; }
        [StringLength(500, ErrorMessage = "Ghi chú quan trọng không được vượt quá 500 ký tự.")]
        public string? ImportantNote { get; set; }
        public IFormFile? ImageUrl { get; set; }
    }
}
