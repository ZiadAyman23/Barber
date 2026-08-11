using System;
using System.Collections.Generic;
using System.Text;

namespace Core.IRepository
{
    public interface ISalaryRepo
    {
        public Task AddSalary(Salaries salaries);
        public Task<List<Salaries>> GetSalaryByBarberId(string barberId, Pagenation pagenation);
        public Task<List<Salaries>> GetAllSalaries(Pagenation pagenation);
        public Task<Salaries> GetSalaryById(string salaryId);
        public Task<CheckLastSalary> GetCuurentSalaries(string barberId);
        public Task<(decimal salary,decimal deduction)> GetMonthelyReport(DateTime start , DateTime end);
        public Task<(decimal salaries , decimal deduction)> GetMonthelyBarberSalares(GenralDuration genral);

    }
}
