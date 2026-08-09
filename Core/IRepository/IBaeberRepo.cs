namespace Core.IRepository
{
    public interface IBaeberRepo
    {
        public Task AddBarber(Barber barberD);
        public Task RemoveBarber(string barberId);
        public Task EditBarber(Barber barberD);
        public Task<Barber> GetBarber(string barberId);
        public Task<List<Barber>> GetAllBarbers();
    }
}
