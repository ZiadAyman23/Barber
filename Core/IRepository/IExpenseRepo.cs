global using Core.Models;

namespace Core.IRepository
{
    public interface IExpenseRepo
    {
        public Task<List<Expense>> GetAllExpenses(Pagenation pagenation);
        public Task<Expense> GetExpenses(string id);
        public Task AddExpense(Expense expense);
        public Task RemoveExpense(string id);
        public Task UpdateExpense(Expense expense);
        public Task<List<Expense>> GetByDate(DateOnly date);
        public Task<decimal> GetMonthelyReport(DateTime start, DateTime end);

    }
}
