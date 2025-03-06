#  How to Generate and Apply Migrations for Appointment Management Backend

This guide provides a step-by-step approach for generating and applying migrations in the **Appointment Management Backend** using **Entity Framework Core**.

## **Prerequisites**
Ensure you have the following installed on your machine:

- .NET 8 SDK
- Microsoft SQL Server (LocalDB, SQL Server Express, or a full instance)
- Entity Framework Core CLI Tools
- A valid connection string for SQL Server

## ** Cloning the Repository**
First, clone the repository to your local machine:

```sh
git clone https://github.com/YOUR_GITHUB_USERNAME/AppointmentManagement.Backend.git
cd AppointmentManagement.Backend
```

## ** Step 1: Ensure EF Core Tools Are Installed**
Run the following command to check if the **EF Core CLI Tools** are installed:

```sh
dotnet tool list -g
```

If `dotnet-ef` is **not installed**, install it using:

```sh
dotnet tool install --global dotnet-ef
```

## ** Step 2: Restore Dependencies**
Before running migrations, restore all required dependencies:

```sh
dotnet restore
```

## ** Step 3: Configure the Database Connection**
Open `src/AppointmentManagement.Api/appsettings.json` and update the **database connection string** to match your local SQL Server instance:

```json
"ConnectionStrings": {
  "DefaultConnection": "Server=YOUR_SERVER;Database=AppointmentDb;Trusted_Connection=True;TrustServerCertificate=True;"
}
```

- Replace `YOUR_SERVER` with your local SQL Server instance name (e.g., `localhost\SQLEXPRESS`).
- If you're using **SQL Server LocalDB**, use:
  ```json
  "Server=(localdb)\MSSQLLocalDB;Database=AppointmentDb;Trusted_Connection=True;"
  ```

## ** Step 4: Generate a New Migration**
Navigate to the project root and generate a new migration:

```sh
dotnet ef migrations add InitialCreate --project src/AppointmentManagement.Infrastructure --startup-project src/AppointmentManagement.Api
```

If successful, you should see a new migration file under:

```
src/AppointmentManagement.Infrastructure/Migrations/
```

## ** Step 5: Apply Migrations and Update Database**
Run the following command to apply migrations and create the database:

```sh
dotnet ef database update --project src/AppointmentManagement.Infrastructure --startup-project src/AppointmentManagement.Api
```

This will:
- Create the **AppointmentDb** database in your SQL Server.
- Apply all defined migrations.
- Seed initial data (roles, admin user, etc.).

## ** Step 6: Verify Database and Seeded Data**
After running the migration, check if the data has been successfully seeded:

1. Open **SQL Server Management Studio (SSMS)** or **Azure Data Studio**.
2. Run the following SQL queries:

```sql
SELECT * FROM Roles;
SELECT * FROM Users;
```

You should see:
- **Roles Table:** Contains `User` and `Manager` roles.
- **Users Table:** Contains the default `admin` user.

## ** Additional EF Core Commands**

- **View Pending Migrations:**
  ```sh
  dotnet ef migrations list --project src/AppointmentManagement.Infrastructure --startup-project src/AppointmentManagement.Api
  ```
- **Remove Last Migration (if not applied):**
  ```sh
  dotnet ef migrations remove --project src/AppointmentManagement.Infrastructure --startup-project src/AppointmentManagement.Api
  ```
- **Reapply Migrations:**
  ```sh
  dotnet ef database update --project src/AppointmentManagement.Infrastructure --startup-project src/AppointmentManagement.Api
  ```

## ** Done!**
You have successfully set up the **Appointment Management Backend** database and applied migrations. Now, you can run the application:

```sh
dotnet run --project src/AppointmentManagement.Api
```

Visit **`http://localhost:5000/swagger`** to explore the API.

Happy coding! 