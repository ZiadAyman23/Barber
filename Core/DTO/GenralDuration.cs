using System;
using System.Collections.Generic;
using System.Text;

namespace Core.DTO
{
    public class GenralDuration
    {
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string BarberId { get; set; } = string.Empty;
    }
}
