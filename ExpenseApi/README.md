# ExpenseApi - Expense Tracker Backend

The backend API component of the Expense Tracker application, built with .NET 8.

## Technologies

- .NET 8.0
- Entity Framework Core with MySQL
- Swagger for API documentation
- DotNetEnv for environment variables
- User Secrets for secure configuration

## Prerequisites

- .NET SDK 8.0+
- MySQL Server
- Entity Framework Core CLI tools

## Database Setup

1. Install EF Core CLI tools if not already installed:

   ```
   dotnet tool install --global dotnet-ef
   ```

2. Create a MySQL database named `expensetracker`

3. Configure your connection string using one of these methods:
   - User secrets (recommended for development)
   - Environment variables
   - .env file

## Configuration

### Using User Secrets (Development)

1. Initialize user secrets (if not already done):

   ```
   cd ExpenseApi/ExpenseApi
   dotnet user-secrets init
   ```

2. Set your database connection string:
   ```
   dotnet user-secrets set "ConnectionStrings:ExpenseDbContext" "Server=localhost;Database=expensetracker;Uid=YOUR_USERNAME;Pwd=YOUR_PASSWORD;"
   ```

### Using Environment Variables

Create a `.env` file in the ExpenseApi/ExpenseApi directory with:

```
ConnectionStrings__ExpenseDbContext=Server=localhost;Database=expensetracker;Uid=YOUR_USERNAME;Pwd=YOUR_PASSWORD;
```

## Running the Application

1. Navigate to the API project directory:

   ```
   cd ExpenseApi/ExpenseApi
   ```

2. Apply database migrations:

   ```
   dotnet ef database update
   ```

3. Start the API:
   ```
   dotnet run
   ```

The API will be available at:

- API: [http://localhost:5111/api](http://localhost:5111/api)
- Swagger UI: [http://localhost:5111/swagger](http://localhost:5111/swagger)

## Project Structure

- `Controllers/` - API endpoint controllers
- `Data/` - Database context and configuration
- `Models/` - Domain models
- `Services/` - Business logic services
- `Migrations/` - EF Core migrations

## API Endpoints

| Method | Endpoint               | Description              |
| ------ | ---------------------- | ------------------------ |
| GET    | /api/transactions      | Get all transactions     |
| GET    | /api/transactions/{id} | Get transaction by ID    |
| POST   | /api/transactions      | Create a new transaction |
| PUT    | /api/transactions/{id} | Update a transaction     |
| DELETE | /api/transactions/{id} | Delete a transaction     |

## Database Migrations

To create a new migration after model changes:

```
dotnet ef migrations add MigrationName
```

To apply migrations to the database:

```
dotnet ef database update
```

## Security Notes

- Sensitive data like connection strings are stored using user secrets or environment variables
- CORS is configured to allow requests from the Vue.js frontend
- Authentication and authorization features can be added as needed
