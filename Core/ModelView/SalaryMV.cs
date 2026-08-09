using System;
using System.Collections.Generic;
using System.Text;

namespace Core.ModelView
{
    public class SalaryMV
    {
        public string ID { get; set; }=string.Empty;
        public DateTime startDate { get; set; }
        public DateTime endDate { get; set; }
        public decimal salary { get; set; }
        public decimal Deducation { get; set; }
        public decimal finalSalary { get; set; }
        public string BarberId { get; set; } = string.Empty;
        public string BarberName { get; set; } =string.Empty;

    }
}
