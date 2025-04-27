using System.Collections.Generic;
using System.Threading.Tasks;
using ExpenseApi.Models;

namespace ExpenseApi.Services
{
    public interface ITransactionService
    {
        // CRUD operations for transactions
        Task<IEnumerable<Transaction>> GetTransactionsAsync();
        Task<Transaction?> GetTransactionAsync(int id);
        Task<Transaction> CreateTransactionAsync(Transaction transaction);
        Task<bool> UpdateTransactionAsync(int id, Transaction transaction);
        Task<bool> DeleteTransactionAsync(int id);

        // get expenses and incomes separately (might be refactored later to use query parameters)
        Task<IEnumerable<Transaction>> GetExpensesAsync();
        Task<IEnumerable<Transaction>> GetIncomesAsync();
    }
}
