using System;
using System.Collections.Generic;
using System.Text;

namespace Core.ModelView
{
    public class BarberMonthelyReportMV
    {
        public string BarberName { get; set; } = string.Empty;
        public int BarberOrder { get; set; }
        public int BarberAttendance { get; set; }
        public decimal BarberSalaries { get; set; }
        public decimal BarberDeduction { get; set; }
        public decimal BarberNetSalaries { get; set; }

    }
}
