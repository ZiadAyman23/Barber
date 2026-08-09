using Infrastructer.Context;
using System;
using System.Collections.Generic;
using System.Text;

namespace Infrastructer.Repository
{
    public class SalaryRepo : ISalaryRepo
    {
        private readonly DataContext dataContext;

        public SalaryRepo(DataContext dataContext)
        {
            this.dataContext = dataContext;
        }


        public async Task<CheckLastSalary> GetCuurentSalaries(string barberId)
        {
            var salaries =await dataContext.Salaries.Where(s => s.BarberID == barberId).OrderByDescending(s => s.EffectiveDate).ToListAsync();
            if (salaries == null || salaries.Count == 0)
            {
                return new CheckLastSalary
                {
                    IsLastSalary = false,
                    LastSalaryDate = DateTime.MinValue
                };
            }
            return new CheckLastSalary
            {
                IsLastSalary = true,
                LastSalaryDate = salaries.First().EffectiveDate
            };
        }

        async Task ISalaryRepo.AddSalary(Salaries salaries)
        {
            await dataContext.Salaries.AddAsync(salaries);
            await dataContext.SaveChangesAsync();
        }

        async Task<List<Salaries>> ISalaryRepo.GetAllSalaries(Pagenation pagenation)
        {
           var salaries =await dataContext.Salaries.Include(s => s.Barber).OrderByDescending(s => s.EffectiveDate)
                .Skip((pagenation.PageNumber - 1) * pagenation.PageSize).Take(pagenation.PageSize).AsNoTracking().ToListAsync();
            return salaries;
        }

        async Task<List<Salaries>> ISalaryRepo.GetSalaryByBarberId(string barberId, Pagenation pagenation)
        {
            var salary = await dataContext.Salaries.Include(s => s.Barber).Where(e=>e.BarberID == barberId).OrderByDescending(e=>e.EffectiveDate)
                .Skip((pagenation.PageNumber - 1) * pagenation.PageSize).Take(pagenation.PageSize).AsNoTracking().ToListAsync();
            return salary;
        }

        async Task<Salaries> ISalaryRepo.GetSalaryById(string salaryId)
        {
            var salary = await dataContext.Salaries.Include(s => s.Barber).FirstOrDefaultAsync(s => s.Id == salaryId);
            if (salary == null)
                throw new KeyNotFoundException(nameof(salary));

            return salary;
        }
    }
}
