using System;
using System.Collections.Generic;
using System.Text;

namespace Core.ModelView
{
    public class MonthelyReportMV
    {
        public enum Month
        {
            January = 1,
            February = 2,
            March = 3,
            April = 4,
            May = 5,
            June = 6,
            July = 7,
            August = 8,
            September = 9,
            October = 10,
            November = 11,
            December = 12
        }
        public int orders {  get; set; }
       public decimal totalSales { get; set; }
        public decimal TotalSalaries { get; set; }
        public decimal TotalDeductions { get; set; }
        public decimal TotalExpenses { get; set; }
        public decimal Profit { get; set; }
        public Month month { get; set; }
        public int Year { get; set; }
        public List<BarberMonthelyReportMV> Barbers { get; set; }= new List<BarberMonthelyReportMV>();

    }
}
