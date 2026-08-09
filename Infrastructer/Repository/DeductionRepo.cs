using Infrastructer.Context;

namespace Infrastructer.Repository
{
    public class DeductionRepo : IDeductionRepo
    {
        private readonly DataContext dataContext;

        public DeductionRepo(DataContext dataContext)
        {
            this.dataContext = dataContext;
        }

    

        public async Task<decimal> GetDeductionByBarberId(string barberId, DateTime date)
        {
            DateTime startDaed = date;
            DateTime endDate = DateTime.Now;
            var deductions =await dataContext.Deductions.Where(e => e.BarberId == barberId && e.Date > startDaed && e.Date <= endDate).SumAsync(e => e.Amount);
            return deductions;
        }

        async Task IDeductionRepo.AddDeduction(Deductions deductions)
        {
            dataContext.Deductions.Add(deductions);
            await dataContext.SaveChangesAsync();
        }

        async Task IDeductionRepo.DeleteDeduction(string deductionId)
        {
            var deduction = await dataContext.Deductions.FirstOrDefaultAsync(e => e.Id == deductionId);
            if (deduction == null)
                throw new KeyNotFoundException("");

            dataContext.Deductions.Remove(deduction);
            await dataContext.SaveChangesAsync();
        }

        async Task<List<Deductions>> IDeductionRepo.GetAllBarberDeduction(string deductionId, Pagenation pagenation)
        {

            var deductions = await dataContext.Deductions.Where(e => e.BarberId == deductionId).OrderByDescending(e => e.Date)
            .Skip((pagenation.PageNumber - 1) * pagenation.PageSize).Take(pagenation.PageSize).AsNoTracking().ToListAsync();

            return deductions;

        }

        async Task<List<Deductions>> IDeductionRepo.GetAllDeductionByDate(DateTime date)
        {
            var startDate = date.Date;
            var endDate = date.Date.AddDays(1);
            var deductions = await dataContext.Deductions.Include(e => e.Barber).Where(e => e.Date >= startDate && e.Date < endDate ).AsNoTracking().ToListAsync();


            return deductions;
        }

        async Task<List<Deductions>> IDeductionRepo.GetAllDeductions(Pagenation pagenation)
        {
            var deductions = await dataContext.Deductions.Include(e => e.Barber).OrderByDescending(e => e.Date)
               .Skip((pagenation.PageNumber - 1) * pagenation.PageSize).Take(pagenation.PageSize).AsNoTracking().ToListAsync();

            return deductions;
        }


        async Task<Deductions> IDeductionRepo.GetDeduction(string deductionId)
        {
            var deduction = await dataContext.Deductions.FirstOrDefaultAsync(e => e.Id == deductionId);
            if (deduction == null)
                throw new KeyNotFoundException("Not Found");
            return deduction;
        }


        async Task IDeductionRepo.UpdateDeduction(Deductions deductions)
        {

            await dataContext.SaveChangesAsync();
        }


        public async Task<List<Deductions>> GetAllDeductionByDateRange( GenralDuration duration)
        {
           

            var deductions =await dataContext.Deductions.Where(e => e.BarberId == duration.BarberId && e.Date > duration.StartDate && e.Date <= duration.EndDate)
                .OrderByDescending(e => e.Date).AsNoTracking().ToListAsync();
            return deductions;
        }
    }
}
