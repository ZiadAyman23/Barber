using System;
using System.Collections.Generic;
using System.Text;

namespace Core.DTO
{
    public class RegisterD
    {
        [Required]
        public string UserName { get; set; } =string.Empty;
        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }=string.Empty;
        [Required]
        [DataType(DataType.Password)]
        [Compare("Password",ErrorMessage ="Not Match To Password")]
        public string ConfirmPassword {  get; set; }=string.Empty;
        [Required]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; } = string.Empty;
    }
}