global using Core.IService;

namespace Infrastructer.Service
{
    public class BarberService : IBarberService
    {
        private readonly IBaeberRepo baeberRepo;

        public BarberService(IBaeberRepo baeberRepo)
        {
            this.baeberRepo = baeberRepo;
        }


        async Task IBarberService.AddBarber(BarberD barberD)
        {
            Barber barber = new Barber()
            {
                Name = barberD.Name,
                HisPercent = barberD.HisPercent,
                DailyPrice = barberD.DailyPrice
            };

           await baeberRepo.AddBarber(barber);

        }

        async Task IBarberService.EditBarber(string barberId, BarberD barberD)
        {
            var _barber =await baeberRepo.GetBarber(barberId);

            _barber.Name = barberD.Name;
            _barber.HisPercent = barberD.HisPercent;
            _barber.DailyPrice = barberD.DailyPrice;

           await baeberRepo.EditBarber(_barber);
        }

        async Task<List<BarberMV>> IBarberService.GetAllBarbers()
        {
           var data= await baeberRepo.GetAllBarbers();
            List<BarberMV> barberMVs = new List<BarberMV>();
            foreach (var barber in data)
            {
                BarberMV barberMV = new BarberMV()
                {
                    Id = barber.Id,
                    Name = barber.Name,
                    HisPercent = barber.HisPercent,
                    DailyPrice = barber.DailyPrice,
                    StartDate = barber.StartDate,
                    NumberOfDeductions = barber.Deductions.Where(d => d.Date == DateTime.Now).Count(),
                    sAbsentToday = barber.Attendances.Where(a => a.Date == DateOnly.FromDateTime(DateTime.Now)).Select(a => a.IsAddent.ToString()).FirstOrDefault() ?? "Apsent"
                };
                barberMVs.Add(barberMV);
            }
            return barberMVs;
        }

        async Task<BarberMV> IBarberService.GetBarber(string barberId)
        {

            var barber =await baeberRepo.GetBarber(barberId);
            BarberMV barberMV = new BarberMV()
            {
                Id = barber.Id,
                Name = barber.Name,
                HisPercent = barber.HisPercent,
                DailyPrice = barber.DailyPrice,
                StartDate = barber.StartDate,

                NumberOfDeductions = barber.Deductions.Where(d => d.Date == DateTime.Now).Count(),
                sAbsentToday = barber.Attendances.Where(a => a.Date == DateOnly.FromDateTime(DateTime.Now)).Select(a => a.IsAddent.ToString()).FirstOrDefault() ?? "Apsent"
            };
            return barberMV;
        }


        async Task IBarberService.RemoveBarber(string barberId)
        {
          await  baeberRepo.RemoveBarber(barberId);
        }
    }
}
