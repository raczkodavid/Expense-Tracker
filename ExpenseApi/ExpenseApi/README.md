# Expense Tracker API

A RESTful API for tracking expenses built with ASP.NET Core.

## Prerequisites

- [.NET SDK 8.0](https://dotnet.microsoft.com/download) or later
- [MySQL Server](https://dev.mysql.com/downloads/mysql/)

## Getting Started

### Database Setup

1. Install and start MySQL Server
2. Create a database named `expensetracker`
3. Configure your connection string:

   - Create a `.env` file in the project root (copy from `.env.example`)

     ```
     # Database Configuration
     ConnectionStrings__ExpenseDbContext=Server=localhost;Database=expensetracker;Uid=YOUR_USERNAME;Pwd=YOUR_PASSWORD;
     ```

   - The default connection string in the provided `.env` file uses:
     ```
     Server=localhost;Database=expensetracker;Uid=root;Pwd=root;
     ```
     You can modify this in the `.env` file to match your MySQL credentials.

### Install Entity Framework Tools

```powershell
dotnet tool install --global dotnet-ef
```

### Create Database Schema

Run the migrations to create the database schema:

```powershell
dotnet ef database update
```

### Run the Application

```powershell
dotnet run
```

The API will be available at:

- http://localhost:5113/api

Swagger UI is available at:

- http://localhost:5113/swagger

> **Note:** The application uses these specific ports by default, configured in the `Properties/launchSettings.json` file.
> If you need to change the ports, modify the `applicationUrl` property in the `http` and `https` profiles.

## Project Structure

- **Controllers/** - API endpoints
- **Data/** - Database context and seeders
- **Models/** - Entity models
- **Services/** - Business logic
