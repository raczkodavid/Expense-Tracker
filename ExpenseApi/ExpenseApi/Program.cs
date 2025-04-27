using System;
using ExpenseApi.Data;
using ExpenseApi.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

string allowVueOrigin = "_allowVueOrigin";

var builder = WebApplication.CreateBuilder(args);

// Load environment variables from .env file in development
if (builder.Environment.IsDevelopment())
{
    DotNetEnv.Env.Load();
    
    // Add user secrets in development
    builder.Configuration.AddUserSecrets<Program>();
}

// Add the db context using mysql
builder.Services.AddDbContext<ExpenseContext>(options =>
    options.UseMySql(
        builder.Configuration.GetConnectionString("ExpenseDbContext")
            ?? throw new InvalidOperationException("Connection string 'ExpenseTrackerContext' not found."),
        ServerVersion.AutoDetect(builder.Configuration.GetConnectionString("ExpenseDbContext"))
    )
);

// Add CORS policy to allow Vue.js frontend
builder.Services.AddCors(options =>
{
    options.AddPolicy(name: allowVueOrigin,
        builder =>
        {
            builder.AllowAnyOrigin()
                .AllowAnyHeader()
                .AllowAnyMethod();
        });
});

// register custom services
builder.Services.AddScoped<ITransactionService, TransactionService>();


builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Use CORS policy
app.UseCors(allowVueOrigin);

// Seed the database on startup
using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<ExpenseContext>();
    await DbSeeder.SeedAsync(dbContext); // Run seeder logic here
}


// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
