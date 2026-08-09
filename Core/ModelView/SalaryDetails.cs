using System;
using System.Collections.Generic;
using System.Text;

namespace Core.ModelView
{
    public class SalaryDetails
    {
        public string BarberId { get; set; }=string.Empty;
        public string BarberName { get; set; }= string.Empty;
        public decimal Salary { get; set; }
        public decimal Deduction { get; set; }
        public decimal FinalSalary { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public List<AttendanceMV> Attendances { get; set; } = new List<AttendanceMV>();
        public List<OrderMV> orderMVs { get; set; }= new List<OrderMV>();
        public List<DeductionMV> DeductionMVs { get; set; } = new List<DeductionMV>();
    }
}
