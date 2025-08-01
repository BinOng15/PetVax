﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetVax.BusinessObjects.DTO.VaccinationSchedule
{
    public class CreateVaccinationScheduleDTO
    {
        [Required(ErrorMessage = "Bệnh không được bỏ trống.")]
        [Range(1, int.MaxValue, ErrorMessage = "Bệnh phải là một ID hợp lệ.")]
        public int DiseaseId { get; set; }
        [Required(ErrorMessage = "Loài của thú cưng không được bỏ trống.")]
        [StringLength(50, ErrorMessage = "Loài của thú cưng không được vượt quá 50 ký tự.")]
        [RegularExpression(@"^(Dog|Cat)$", ErrorMessage = "Loài của thú cưng chỉ được phép là 'Dog' hoặc 'Cat'.")]
        public string Species { get; set; }
        [Required(ErrorMessage = "Mũi tiêm không được bỏ trống.")]
        public int DoseNumber { get; set; }
        [Required(ErrorMessage = "Độ tuổi của thú cưng không được bỏ trống.")]
        [Range(1, 1043, ErrorMessage = "Độ tuổi của thú cưng phải nằm trong khoảng từ 1 đến 1043 tuần (tức là từ 0 tới 20 tuổi). (Lưu ý: Độ tuổi được tính theo tuần)")]
        public int AgeInterval { get; set; }
    }
}
