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
    }
}
