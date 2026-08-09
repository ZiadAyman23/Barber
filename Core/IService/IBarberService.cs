namespace Core.IService
{
    public interface IBarberService
    {
        public Task AddBarber(BarberD barberD);
        public Task RemoveBarber(string barberId);
        public Task EditBarber(string barberId, BarberD barberD);
        public Task<BarberMV> GetBarber(string barberId);
        public Task<List<BarberMV>> GetAllBarbers();
    }
}
