namespace Infrastructer.Service
{
    public class DeductionServes : IDeductionService
    {

        private readonly IDeductionRepo deductionRepo1;
        public DeductionServes(IDeductionRepo deductionRepo)
        {
            deductionRepo1 = deductionRepo;
        }

        async Task IDeductionService.AddDeduction(DeductionD deductionD)
        {
            Deductions deductions = new Deductions();

            deductions.Amount = deductionD.Amount;
            deductions.CreatedBy = deductionD.CreatedBy;
            deductions.BarberId = deductionD.BarberId;
            deductions.Reason = deductionD.Reason;

            await deductionRepo1.AddDeduction(deductions);

        }

        async Task IDeductionService.EditDeduction(string id, DeductionD deductions)
        {
            var deduction = await deductionRepo1.GetDeduction(id);

            deduction.Amount = deductions.Amount;
            deduction.CreatedBy = deductions.CreatedBy;
            deduction.BarberId = deductions.BarberId;
            deduction.Reason = deductions.Reason;

            await deductionRepo1.UpdateDeduction(deduction);

        }

        async Task<List<DeductionMV>> IDeductionService.GetAllBarberDeductions(string id, Pagenation pagenation)
        {
            if (pagenation.PageNumber < 1)
                pagenation.PageNumber = 1;

            if (pagenation.PageSize < 1)
                pagenation.PageSize = 1;

            var deductions = await deductionRepo1.GetAllBarberDeduction(id, pagenation);

            List<DeductionMV> result = new List<DeductionMV>();
            foreach (var d in deductions)
            {
                result.Add(new DeductionMV()
                {
                    Amount = d.Amount,
                    Reason = d.Reason,
                    CreatedBy = d.CreatedBy,
                    DateOnly = d.Date,
                    id = d.Id,
                });
            }

            return result;
        }


        async Task<List<DeductionMV>> IDeductionService.GetAllDeduction(Pagenation pagenation)
        {

            if (pagenation.PageNumber < 1)
                pagenation.PageNumber = 1;

            if (pagenation.PageSize < 1)
                pagenation.PageSize = 1;

            var deductions = await deductionRepo1.GetAllDeductions(pagenation);

            List<DeductionMV> result = new List<DeductionMV>();
            foreach (var d in deductions)
            {
                result.Add(new DeductionMV()
                {
                    Amount = d.Amount,
                    Reason = d.Reason,
                    CreatedBy = d.CreatedBy,
                    DateOnly = d.Date,
                    id = d.Id,
                    BaeberName = d.Barber.Name,
                    BarberId = d.Barber.Id,

                });
            }

            return result;

        }

        async Task<List<DeductionMV>> IDeductionService.GetAllDeductionByDated(DateTime date)
        {
            var deductions = await deductionRepo1.GetAllDeductionByDate(date);

            List<DeductionMV> result = new List<DeductionMV>();

            foreach (var d in deductions)
            {
                result.Add(new DeductionMV()
                {
                    Amount = d.Amount,
                    Reason = d.Reason,
                    CreatedBy = d.CreatedBy,
                    DateOnly = d.Date,
                    id = d.Id,
                    BaeberName = d.Barber.Name,
                    BarberId = d.Barber.Id,
                });
            }

            return result;

        }

        async Task IDeductionService.RemoveDeduction(string id)
        {
            await deductionRepo1.DeleteDeduction(id);
        }
    }
}
