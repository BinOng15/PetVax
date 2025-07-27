using PetVax.BusinessObjects.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetVax.BusinessObjects.DTO.AuthenticateDTO
{
    public class AuthResponseDTO
    {
        public int AccountId { get; set; }
        public string Email { get; set; }
        public EnumList.Role Role { get; set; }
        public string AccessToken { get; set; }
        public string RefreshToken { get; set; }
        public DateTime AccessTokenExpiration { get; set; }
        public DateTime RefreshTokenExpiration { get; set; }
        public bool IsVerify { get; set; }
    }
    public class AuthResponseForMobileDTO
    {
        public int AccountId { get; set; }
        public string Email { get; set; }
        public EnumList.Role Role { get; set; }
        public string AccessToken { get; set; }
        public string RefreshToken { get; set; }
        public DateTime AccessTokenExpiration { get; set; }
        public DateTime RefreshTokenExpiration { get; set; }
        public bool IsVerify { get; set; }
        public string? FullName { get; set; }
        public string? Image { get; set; }
    }
}
