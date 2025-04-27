using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Transactions;
using ExpenseApi.Data;
using ExpenseApi.Models;
using Microsoft.EntityFrameworkCore;

namespace ExpenseApi.Services
{
    public class TransactionService : ITransactionService
    {
        private ExpenseContext _context;

        public TransactionService(ExpenseContext context)
        {
            _context = context;
        }

        public async Task<Models.Transaction> CreateTransactionAsync(Models.Transaction transaction)
        {
            _context.Transactions.Add(transaction);
            await _context.SaveChangesAsync();
            return transaction;
        }

        public async Task<bool> DeleteTransactionAsync(int id)
        {
            var transaction = await _context.Transactions.FindAsync(id);
            if (transaction == null)
                return false;

            _context.Transactions.Remove(transaction);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<IEnumerable<Models.Transaction>> GetExpensesAsync()
        {
            return await _context.Transactions
                .Where(t => t.Type == TransactionType.Expense)
                .ToListAsync();
        }

        public async Task<IEnumerable<Models.Transaction>> GetIncomesAsync()
        {
            return await _context.Transactions
                .Where(t => t.Type == TransactionType.Income)
                .ToListAsync();
        }

        public async Task<Models.Transaction?> GetTransactionAsync(int id)
        {
            return await _context.Transactions.FindAsync(id);
        }

        public async Task<IEnumerable<Models.Transaction>> GetTransactionsAsync()
        {
            return await _context.Transactions.ToListAsync();
        }

        public async Task<bool> UpdateTransactionAsync(int id, Models.Transaction transaction)
        {
            // find the tracked entity
            var target = await GetTransactionAsync(id);
            if (target == null)
                return false;

            // manually update the fields
            target.Amount = transaction.Amount;
            target.Description = transaction.Description;
            target.Date = transaction.Date;
            target.Category = transaction.Category;
            target.Type = transaction.Type;

            // save the changes
            await _context.SaveChangesAsync();
            return true;
        }

    }
}
