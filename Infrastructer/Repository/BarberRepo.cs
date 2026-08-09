using Infrastructer.Context;

namespace Infrastructer.Repository
{
    public class BarberRepo : IBaeberRepo
    {
        private readonly DataContext dataContext;

        public BarberRepo(DataContext dataContext)
        {
            this.dataContext = dataContext;
        }

        async Task IBaeberRepo.AddBarber(Barber barberD)
        {
            if (barberD == null)
            {
                throw new ArgumentNullException(nameof(barberD));
            }
            dataContext.Barbers.Add(barberD);
            await dataContext.SaveChangesAsync();
        }

        async Task IBaeberRepo.EditBarber(Barber barberD)
        {
            if (barberD == null)
            {
                throw new ArgumentNullException(nameof(barberD));
            }
            dataContext.Barbers.Update(barberD);
            await dataContext.SaveChangesAsync();
        }

        async Task<List<Barber>> IBaeberRepo.GetAllBarbers()
        {
            var barbers =await dataContext.Barbers.Include(b => b.Orders).Include(b => b.Deductions).Include(b => b.Attendances).ToListAsync();
            return barbers;
        }

        async Task<Barber> IBaeberRepo.GetBarber(string barberId)
        {
            var barber = await dataContext.Barbers.FirstOrDefaultAsync(b => b.Id == barberId);
            if (barber == null)
                throw new KeyNotFoundException($"Barber with ID {barberId} not found at database.");
            return barber;
        }

        async Task IBaeberRepo.RemoveBarber(string barberId)
        {
            var barber = await dataContext.Barbers.FirstOrDefaultAsync(b => b.Id == barberId);
            if (barber == null)
                throw new KeyNotFoundException($"Barber with ID {barberId} not found at database.");

            dataContext.Barbers.Remove(barber);
            await dataContext.SaveChangesAsync();
        }

    }
}
