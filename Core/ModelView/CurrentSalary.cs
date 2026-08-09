using System;
using System.Collections.Generic;
using System.Text;

namespace Core.ModelView
{
    public class CurrentSalary
    {
        public string BarberName { get; set; } = string.Empty; 
        public decimal SalaryAmount { get; set; }
        public decimal DeductionAmount { get; set; }
        public decimal TotalSalary { get; set; }
        public int OrderCount { get; set; }
        public int AttendanceCount { get; set; }
        public DateTime LastUpdated { get; set; }
        public string BarberID { get; set; } = string.Empty;
    }
}
