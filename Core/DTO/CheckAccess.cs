using System;
using System.Collections.Generic;
using System.Text;

namespace Core.DTO
{
    public class CheckAccess
    {
        [Required]
        public string pass {  get; set; }=string.Empty;
    }
}
