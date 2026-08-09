using System;
using System.Collections.Generic;
using System.Text;

namespace Core.DTO
{
    public class SalaryD
    {
        public decimal SalaryAmount { get; set; }
        public string BarberID { get; set; } = string.Empty;
        public decimal DeductionAmount { get; set; }
        public decimal TradeAmount { get; set; }
        public DateTime LastUpdated { get; set; }
    }
}
