using Infrastructer.Context;

namespace Infrastructer.Repository
{
    public class ExpeseRepo : IExpenseRepo
    {
        private readonly DataContext dataContext;

        public ExpeseRepo(DataContext dataContext)
        {
            this.dataContext = dataContext;
        }

        public async Task AddExpense(Expense expense)
        {
            dataContext.Expenses.Add(expense);
            await dataContext.SaveChangesAsync();
        }

        public async Task<List<Expense>> GetAllExpenses(Pagenation pagenation)
        {
            var list = await dataContext.Expenses
                .OrderByDescending(e => e.Date)
                .Skip((pagenation.PageNumber - 1) * pagenation.PageSize)
                .Take(pagenation.PageSize)
                .AsNoTracking()
                .ToListAsync();
            return list;
        }

        public async Task<List<Expense>> GetByDate(DateOnly date)
        {
            var data = await dataContext.Expenses
                .Where(e => e.Date == date)
                .AsNoTracking()
                .ToListAsync();
            return data;
        }

        public async Task<Expense> GetExpenses(string id)
        {
            var expense = await dataContext.Expenses.FirstOrDefaultAsync(x => x.Id == id);
            if (expense == null)
                throw new Exception("Expense not found at Database");
            return expense;
        }

        public async Task RemoveExpense(string id)
        {
            var expense = await dataContext.Expenses.FirstOrDefaultAsync(x => x.Id == id);
            if (expense == null)
                throw new Exception("Expense not found at Database");

            dataContext.Expenses.Remove(expense);
            await dataContext.SaveChangesAsync();
        }

        public async Task UpdateExpense(Expense expense)
        {
            dataContext.Expenses.Update(expense);
            await dataContext.SaveChangesAsync();
        }
    }
}
