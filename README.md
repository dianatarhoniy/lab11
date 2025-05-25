# ClinicSolution

Minimal reference implementation of the **prescription / patient** exercise.

## Quick start

```bash
cd ClinicSolution
dotnet restore        # fetch packages
dotnet test           # run unit tests
dotnet run --project src/WebApi
```

Then browse Swagger at <https://localhost:5001/swagger>.

*Default connection string* points to localdb; change it in `appsettings.json` or via environment variable.

## Structure

* **Domain** – POCO entities only  
* **Infrastructure** – EF Core `AppDbContext`  
* **Application** – DTOs + Services with business rules  
* **WebApi** – thin controllers & Program  
* **tests** – xUnit + In‑Memory EFCore  
