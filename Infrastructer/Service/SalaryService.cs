using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace Infrastructer.Service
{
    public class SalaryService : ISalaryService
    {
        private readonly ISalaryRepo salaryRepo;
        private readonly IBaeberRepo baeberRepo;
        private readonly IOrderRepo orderRepo;
        private readonly IDeductionRepo deductionRepo;
        private readonly IAttendanceRepo attendanceRepo;

        public SalaryService(ISalaryRepo salaryRepo,IBaeberRepo baeberRepo,IOrderRepo orderRepo,IDeductionRepo deductionRepo , IAttendanceRepo attendanceRepo)
        {
            this.salaryRepo = salaryRepo;
            this.baeberRepo = baeberRepo;
            this.orderRepo = orderRepo;
            this.deductionRepo = deductionRepo;
            this.attendanceRepo = attendanceRepo;
        }

        public async Task<List<CurrentSalary>> GetCuurentSalaries()
        {
            var barbers =await baeberRepo.GetAllBarbers();
            List<CurrentSalary> salaries = new List<CurrentSalary>();
            foreach (var item in barbers)
            {
                if (item.HisPercent.HasValue && !item.DailyPrice.HasValue)
                {
                    var salary =await salaryRepo.GetCuurentSalaries(item.Id);
                    decimal deductionAmount;
                    DateTime startDate;
                    if (salary.IsLastSalary)
                    {
                        // Do something with the last salary
                        startDate = salary.LastSalaryDate;
                    }
                    else
                    {
                        // Handle the case where there is no last salary
                        startDate = item.StartDate.ToDateTime(TimeOnly.MinValue);
                    }
                    var sal = await orderRepo.GetOrdersByBarberId(item.Id, startDate);
                    Console.WriteLine("salary " + sal.Item1 + " count " + sal.Item2);
                    deductionAmount = await deductionRepo.GetDeductionByBarberId(item.Id, startDate);
                    decimal totalSalary = sal.Item1 * (item.HisPercent!.Value / 100m);
                    decimal totalDeduction = deductionAmount;
                    decimal finalSalary = totalSalary - totalDeduction;
                    salaries.Add(new CurrentSalary
                    {
                        BarberName = item.Name,
                        SalaryAmount = totalSalary,
                        DeductionAmount = totalDeduction,
                        TotalSalary = finalSalary,
                        OrderCount =sal.Item2,
                        BarberID = item.Id,
                        LastUpdated = startDate,

                    });
                }
                else if (item.DailyPrice.HasValue && !item.HisPercent.HasValue)
                {
                    var salary = await salaryRepo.GetCuurentSalaries(item.Id);
                    int count;
                    decimal deductionAmount;
                    DateTime dateTime;
                    if (salary.IsLastSalary)
                    {
                        // Do something with the last salary

                        dateTime= salary.LastSalaryDate;
                    }
                    else
                    {
                        // Handle the case where there is no last salary
                        dateTime = item.StartDate.ToDateTime(TimeOnly.MinValue);
                    }
                    count = await attendanceRepo.GetAttendanceCountByBarberId(item.Id, DateOnly.FromDateTime(dateTime));
                    deductionAmount = await deductionRepo.GetDeductionByBarberId(item.Id, dateTime);
                    decimal totalSalary = count * item.DailyPrice!.Value;
                    decimal totalDeduction = deductionAmount;
                    decimal finalSalary = totalSalary - totalDeduction;
                    salaries.Add(new CurrentSalary
                    {
                        BarberName = item.Name,
                        SalaryAmount = totalSalary,
                        DeductionAmount = totalDeduction,
                        TotalSalary = finalSalary,
                        AttendanceCount = count,
                        BarberID = item.Id,
                        LastUpdated = dateTime,
                    });

                }
                else
                {
                    // Handle the case where both HisPercent and DailyPrice are zero or both are non-zero

                    salaries.Add(new CurrentSalary
                    {
                        BarberName = item.Name,
                        SalaryAmount = 0,
                        DeductionAmount = 0,
                        TotalSalary = 0,
                        OrderCount = 0,
                        AttendanceCount = 0,
                        BarberID = item.Id
                    });

                }

            }
            return salaries;
        }

      

        async Task ISalaryService.AddSalary(SalaryD _salary)
        {
            Salaries salary = new Salaries
            {
                SalaryAmount = _salary.SalaryAmount,
                DeductionAmount = _salary.DeductionAmount,
                TradeAmount = _salary.TradeAmount,
                BarberID = _salary.BarberID,
                StartDate = _salary.LastUpdated
                
            };
            await salaryRepo.AddSalary(salary);

        }

        async Task<List<SalaryMV>> ISalaryService.GetAllSalary(Pagenation pagenation)
        {
            var salaries = await salaryRepo.GetAllSalaries(pagenation);
            List<SalaryMV> result = new List<SalaryMV>();
            foreach(var item in salaries)
            {
                result.Add(new SalaryMV
                {
                    ID = item.Id,
                    BarberId = item.BarberID,
                    BarberName = item.Barber.Name,
                    startDate = item.StartDate,
                    endDate = item.EffectiveDate,
                    salary = item.SalaryAmount,
                    Deducation =item.DeductionAmount,
                    finalSalary = item.TradeAmount,
                });
            }

            return result;
            
        }

        async Task<List<SalaryMV>> ISalaryService.GetSalaryByBarberId(string barberId, Pagenation pagenation)
        {
            var salaries = await salaryRepo.GetSalaryByBarberId(barberId , pagenation);
            List<SalaryMV> result = new List<SalaryMV>();
            foreach (var item in salaries)
            {
                result.Add(new SalaryMV
                {
                    ID = item.Id,
                    BarberId = item.BarberID,
                    startDate = item.StartDate,
                    endDate = item.EffectiveDate,
                    salary = item.SalaryAmount,
                    Deducation = item.DeductionAmount,
                    finalSalary = item.TradeAmount,
                    BarberName = item.Barber.Name
                });
            }

            return result;
        }



        public async Task<SalaryDetails> GetSalaryDetails(string salaryId)
        {
            var salaryDetails =await salaryRepo.GetSalaryById(salaryId);
            List<AttendanceMV> attendances = new List<AttendanceMV>();
            List<OrderMV> orders = new List<OrderMV>();
            List<DeductionMV> deductions = new List<DeductionMV>();

            GenralDuration genralDuration = new GenralDuration
            {
                  StartDate = salaryDetails.StartDate,
                  EndDate = salaryDetails.EffectiveDate,
                  BarberId = salaryDetails.BarberID
            };

            var attend = await attendanceRepo.GetDurationAttenWithoutPag(genralDuration);
            foreach (var item in attend)
            {
                attendances.Add(new AttendanceMV
                {
                    AttendanceId = item.Id,
                    Date =item.Date,
                    Status = item.IsAddent.ToString(),
                });
            }

            var ord = await orderRepo.GetOrderByDateRange(genralDuration);
            foreach (var item in ord)
            {
                orders.Add(new OrderMV
                {
                    id = item.Id,
                    Date = item.Date,
                    TotalPrice = item.TotalPrice,
                });
            }

            var ded = await deductionRepo.GetAllDeductionByDateRange(genralDuration);
            foreach (var item in ded)
            {
                deductions.Add(new DeductionMV
                {
                    id = item.Id,
                    Amount = item.Amount,
                    Reason = item.Reason,
                    DateOnly = item.Date,
                    CreatedBy = item.CreatedBy,
                });
            }


            SalaryDetails sal = new SalaryDetails
            {
                BarberId = salaryDetails.BarberID,
                BarberName = salaryDetails.Barber.Name,
                Salary = salaryDetails.SalaryAmount,
                Deduction = salaryDetails.DeductionAmount,
                FinalSalary = salaryDetails.TradeAmount,
                StartDate = salaryDetails.StartDate,
                EndDate = salaryDetails.EffectiveDate,
                Attendances = attendances,
                orderMVs = orders,
                DeductionMVs = deductions
            };

            return sal;
        }

    }

}
