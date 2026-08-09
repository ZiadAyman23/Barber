using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Models
{
    public class Salaries
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public decimal SalaryAmount { get; set; }
        public decimal DeductionAmount { get; set; }
        public decimal TradeAmount { get; set; }
        public DateTime EffectiveDate { get; set; }
        public DateTime StartDate { get; set; }
        public string BarberID { get; set; }= string.Empty;
        public Barber? Barber { get; set; }

    }
}
