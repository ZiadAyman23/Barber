using System;
using System.Collections.Generic;
using System.Text;

namespace Core.IService
{
    public interface ISalaryService
    {
        public Task<List<SalaryMV>> GetAllSalary(Pagenation pagenation);
        public Task AddSalary(SalaryD salary);
        public Task<List<SalaryMV>> GetSalaryByBarberId(string barberId, Pagenation pagenation);
        public Task<List<CurrentSalary>> GetCuurentSalaries();
        public Task<SalaryDetails> GetSalaryDetails(string salaryId);



    }
}
