namespace Infrastructer.Service
{
    public class ExpenseService : IExpenseService
    {
        private readonly IExpenseRepo expenseRepo;

        public ExpenseService(IExpenseRepo expenseRepo)
        {
            this.expenseRepo = expenseRepo;
        }

        public async Task<List<ExpenseMV>> GetByDate(DateOnly date)
        {
            var expenses = await expenseRepo.GetByDate(date);
            return expenses.Select(exp => new ExpenseMV
            {
                Id = exp.Id,
                Amount = exp.Amount,
                CreatedBy = exp.CreatedBy,
                Date = exp.Date,
                Reason = exp.Reason
            }).ToList();
        }

        public async Task AddExpense(ExpenseD expense)
        {
            Expense expense1 = new Expense()
            {
                Amount = expense.Amount,
                CreatedBy = expense.CreatedBy,
                Reason = expense.Reason
            };
            await expenseRepo.AddExpense(expense1);
        }

        public async Task<List<ExpenseMV>> GetAllExpenses(Pagenation pagenation)
        {
            if (pagenation.PageNumber < 1) pagenation.PageNumber = 1;
            if (pagenation.PageSize < 1) pagenation.PageSize = 1;

            var expenses = await expenseRepo.GetAllExpenses(pagenation);
            return expenses.Select(exp => new ExpenseMV
            {
                Id = exp.Id,
                Amount = exp.Amount,
                CreatedBy = exp.CreatedBy,
                Date = exp.Date,
                Reason = exp.Reason
            }).ToList();
        }

        public async Task<ExpenseMV> GetExpenses(string id)
        {
            var expense = await expenseRepo.GetExpenses(id);
            return new ExpenseMV
            {
                Id = expense.Id,
                Amount = expense.Amount,
                CreatedBy = expense.CreatedBy,
                Date = expense.Date,
                Reason = expense.Reason
            };
        }

        public async Task RemoveExpense(string id)
        {
            await expenseRepo.RemoveExpense(id);
        }

        public async Task UpdateExpense(string id, ExpenseD expense)
        {
            var expense1 = await expenseRepo.GetExpenses(id);
            expense1.Amount = expense.Amount;
            expense1.CreatedBy = expense.CreatedBy;
            expense1.Reason = expense.Reason;
            await expenseRepo.UpdateExpense(expense1);
        }
    }
}
