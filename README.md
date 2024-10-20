### Feedback System API
## Overview
The Feedback System API is a web API built with ASP.NET Core, Entity Framework Core, and SQL Server. It allows users to perform CRUD (Create, Read, Update, Delete) operations on feedback entries. Users can filter feedback entries by various criteria, paginate the results, and more.

## Technologies used

- ASP.NET Core 8
- Entity Framework Core
- SQL Server for production
- In-memory database for testing
- xUnit for unit testing

## Features
- Create feedback entries
- Update feedback entries
- Delete feedback entries
- Retrieve paginated and filtered feedback entries

### Db connection 
` "ConnectionStrings": {
  "DefaultConnection": "Server=(localdb)\\mssqllocaldb;Database=FeedbackSystemDb;Trusted_Connection=True;MultipleActiveResultSets=true"
}`

### Drop the existing database (if needed)
`dotnet ef database drop`
### Create a new migration:
`dotnet ef migrations add [name]`
### Apply the migration to update the database
`dotnet ef database update`