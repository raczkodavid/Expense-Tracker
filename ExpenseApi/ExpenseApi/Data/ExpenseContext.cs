using ExpenseApi.Models;
using Microsoft.EntityFrameworkCore;

namespace ExpenseApi.Data
{
    public class ExpenseContext : DbContext
    {
        public ExpenseContext(DbContextOptions<ExpenseContext> options) : base(options) { }

        // DbSet for each entity (table) in the database
        public DbSet<Transaction> Transactions { get; set; }


    }
}
