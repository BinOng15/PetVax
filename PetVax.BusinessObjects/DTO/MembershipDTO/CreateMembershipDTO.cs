using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetVax.BusinessObjects.DTO.MembershipDTO
{
    public class CreateMembershipDTO
    {
        [Required(ErrorMessage = "Vui lòng nhập tên của membership!")]
        [StringLength(100, ErrorMessage = "Tên membership không được vượt quá 100 ký tự!")]
        public string Name { get; set; }
        [Required(ErrorMessage = "Vui lòng nhập mô tả của membership!")]
        [StringLength(200, ErrorMessage = "Mô tả membership không được vượt quá 200 ký tự!")]
        public string Description { get; set; }
        [Required(ErrorMessage = "Vui lòng nhập điểm cần thiết của membership!")]
        [Range(0, int.MaxValue, ErrorMessage = "Điểm cần thiết phải là một số nguyên không âm!")]
        public int MinPoints { get; set; }
        [Required(ErrorMessage = "Vui lòng nhập lợi ích của membership!")]
        [StringLength(200, ErrorMessage = "Lợi ích membership không được vượt quá 200 ký tự!")]
        public string Benefits { get; set; }
        [Required(ErrorMessage = "Vui lòng nhập rank của membership!")]
        [StringLength(50, ErrorMessage = "Rank membership không được vượt quá 50 ký tự!")]
        public string Rank { get; set; }
    }
}
