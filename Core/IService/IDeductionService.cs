namespace Core.IService
{
    public interface IDeductionService
    {
        public Task AddDeduction(DeductionD deductionD);
        public Task RemoveDeduction(string id);
        public Task EditDeduction(string id, DeductionD deductionD);
        public Task<List<DeductionMV>> GetAllDeduction(Pagenation pagenation);

        public Task<List<DeductionMV>> GetAllBarberDeductions(string id, Pagenation pagenation);

        public Task<List<DeductionMV>> GetAllDeductionByDated(DateTime date);
      


    }
}
