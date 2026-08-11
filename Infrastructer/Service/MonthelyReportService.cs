using System;
using System.Collections.Generic;
using System.Text;

namespace Infrastructer.Service
{
    public class MonthelyReportService : IMonthelyReportService
    {
        private readonly IOrderRepo orderRepo;
        private readonly IDeductionRepo deductionRepo;
        private readonly IBaeberRepo baeberRepo;
        private readonly IExpenseRepo expenseRepo;
        private readonly IAttendanceRepo attendanceRepo;
        private readonly ISalaryRepo salaryRepo;

        public MonthelyReportService(IOrderRepo orderRepo,IDeductionRepo deductionRepo , IBaeberRepo baeberRepo , IExpenseRepo expenseRepo, IAttendanceRepo attendanceRepo , ISalaryRepo salaryRepo)
        {
            this.orderRepo = orderRepo;
            this.deductionRepo = deductionRepo;
            this.baeberRepo = baeberRepo;
            this.expenseRepo = expenseRepo;
            this.attendanceRepo = attendanceRepo;
            this.salaryRepo = salaryRepo;
        }
        public async Task<MonthelyReportMV> GenrateMonthelyReport(int Month, int Year)
        {
            DateTime startDate = new DateTime(Year, Month, 1);
            DateTime endDate = startDate.AddMonths(1);
            GenralDuration genral = new GenralDuration();
            genral.StartDate = startDate;
            genral.EndDate = endDate;

            MonthelyReportMV monthelyReportMV = new MonthelyReportMV();
            monthelyReportMV.month= (MonthelyReportMV.Month)Month;
            monthelyReportMV.Year=Year;

            var barbers = await baeberRepo.GetAllBarbers();
            var Orders= await orderRepo.GetMonthelyReport(startDate, endDate);
            monthelyReportMV.orders = Orders.Count;
            monthelyReportMV.totalSales = Orders.salary;

            monthelyReportMV.TotalExpenses= await expenseRepo.GetMonthelyReport(startDate,endDate);

            var salries = await salaryRepo.GetMonthelyReport(startDate, endDate);
            monthelyReportMV.TotalSalaries = salries.salary;
            monthelyReportMV.TotalDeductions = salries.deduction;

            monthelyReportMV.Profit = monthelyReportMV.totalSales - monthelyReportMV.TotalSalaries - monthelyReportMV.TotalExpenses + monthelyReportMV.TotalDeductions;


            foreach (var bar in barbers)
            {
                genral.BarberId = bar.Id;
                BarberMonthelyReportMV barberMonthelyReportMV = new BarberMonthelyReportMV();
                barberMonthelyReportMV.BarberName = bar.Name;
                barberMonthelyReportMV.BarberAttendance = await attendanceRepo.GetMonthelyBarberAttend(genral);
                barberMonthelyReportMV.BarberOrder = await orderRepo.GetMonthelyBarberOrders(genral);
                var sal = await salaryRepo.GetMonthelyBarberSalares(genral);

                barberMonthelyReportMV.BarberSalaries = sal.salaries;
                barberMonthelyReportMV.BarberDeduction = sal.deduction;
                barberMonthelyReportMV.BarberNetSalaries = sal.salaries -sal.deduction;

                monthelyReportMV.Barbers.Add(barberMonthelyReportMV);
                
            }

            return monthelyReportMV;

        }
    }
}
