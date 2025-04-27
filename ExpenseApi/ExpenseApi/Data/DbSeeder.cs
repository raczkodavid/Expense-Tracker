using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ExpenseApi.Models;

namespace ExpenseApi.Data
{

    // seed the database using bogus data
    public static class DbSeeder
    {
        public static async Task SeedAsync(ExpenseContext context)
        {
            // Apply any pending migrations (creates DB if needed)
            await context.Database.MigrateAsync();

            // only seed if the database is empty
            if (!await context.Transactions.AnyAsync())
                await SeedTransactions(context);
        }

        private static async Task SeedTransactions(ExpenseContext context)
        {
            // Create a faker instance for generating sample data
            var faker = new Bogus.Faker<Transaction>()
                .RuleFor(t => t.Description, f => f.Lorem.Sentence())
                .RuleFor(t => t.Amount, f => f.Finance.Amount(1, 1000))
                .RuleFor(t => t.Date, f => f.Date.Past(1))
                .RuleFor(t => t.Type, f => f.PickRandom<TransactionType>())
                .RuleFor(t => t.Category, (f, t) =>
                {
                    if (t.Type == TransactionType.Income)
                        return f.PickRandom("Salary", "Freelance", "Bonus", "Investment");
                    else
                        return f.Commerce.Categories(1)[0];
                });


            // generate random amount of samples (20-100)
            Random random = new Random();
            int sampleCount = random.Next(20, 100);

            // generate the samples
            var transactions = faker.Generate(sampleCount);

            try
            {
                // add the generated data to the database
                await context.Transactions.AddRangeAsync(transactions);
                await context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                // handle any errors that occur during seeding
                Console.WriteLine($"Error seeding database: {ex.Message}");
            }

        }
    }
}
