namespace Core.IRepository
{
    public interface IDeductionRepo
    {
        public Task AddDeduction(Deductions deductions);
        public Task UpdateDeduction(Deductions deductions);
        public Task DeleteDeduction(string deductionId);
        public Task<Deductions> GetDeduction(string deductionId);
        public Task<List<Deductions>> GetAllDeductions(Pagenation pagenation);
        public Task<List<Deductions>> GetAllBarberDeduction(string deductionId, Pagenation pagenation);
        public Task<List<Deductions>> GetAllDeductionByDate(DateTime date);
        public Task<decimal> GetDeductionByBarberId(string barberId, DateTime date);
        public Task<List<Deductions>> GetAllDeductionByDateRange( GenralDuration duration);


    }
}
