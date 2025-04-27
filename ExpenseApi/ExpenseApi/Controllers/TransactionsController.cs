using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ExpenseApi.Models;
using ExpenseApi.Services;

namespace ExpenseApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TransactionsController : ControllerBase
    {
        private readonly ITransactionService _transactionService;

        public TransactionsController(ITransactionService transactionService)
        {
            _transactionService = transactionService;
        }

        // GET: api/Transactions
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Transaction>>> GetTransactions()
        {
            var transactions = await _transactionService.GetTransactionsAsync();
            return Ok(transactions);
        }

        // GET: api/Transactions/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Transaction>> GetTransaction(int id)
        {
            var transaction = await _transactionService.GetTransactionAsync(id);
            if (transaction == null)
                return NotFound();

            return Ok(transaction);
        }

        // PUT: api/Transactions/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutTransaction(int id, Transaction transaction)
        {
            if (id != transaction.Id)
                return BadRequest();

            var updated = await _transactionService.UpdateTransactionAsync(id, transaction);
            if (!updated)
                return NotFound();

            return NoContent();
        }

        // POST: api/Transactions
        [HttpPost]
        public async Task<ActionResult<Transaction>> PostTransaction(Transaction transaction)
        {
            var createdTransaction = await _transactionService.CreateTransactionAsync(transaction);
            return CreatedAtAction("GetTransaction", new { id = createdTransaction.Id }, createdTransaction);
        }

        // DELETE: api/Transactions/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTransaction(int id)
        {
            var deleted = await _transactionService.DeleteTransactionAsync(id);
            if (!deleted)
                return NotFound();

            return NoContent();
        }

        // GET: api/Transactions/Expenses
        [HttpGet("Expenses")]
        public async Task<ActionResult<IEnumerable<Transaction>>> GetExpenses()
        {
            var expenses = await _transactionService.GetExpensesAsync();
            return Ok(expenses);
        }

        // GET: api/Transactions/Incomes
        [HttpGet("Incomes")]
        public async Task<ActionResult<IEnumerable<Transaction>>> GetIncomes()
        {
            var incomes = await _transactionService.GetIncomesAsync();
            return Ok(incomes);
        }
    }
}
