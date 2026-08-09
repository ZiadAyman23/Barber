global using Core.DTO;
global using Core.ModelView;

namespace Core.IService
{
    public interface IExpenseService
    {
        public Task<List<ExpenseMV>> GetAllExpenses(Pagenation pagenation);
        public Task<ExpenseMV> GetExpenses(string id);
        public Task AddExpense(ExpenseD expense);
        public Task RemoveExpense(string id);
        public Task UpdateExpense(string id, ExpenseD expense);
        public Task<List<ExpenseMV>> GetByDate(DateOnly date);
    }
}
