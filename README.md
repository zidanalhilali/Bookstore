# Bookstore

A book catalog web application built with **ASP.NET Core MVC**, **Entity Framework Core** and **SQL Server**, created as a learning project.

## Features
- CRUD for books and authors (one-to-many relationship)
- Book cover upload with server-side validation (type, size)
- Search by title, author or description
- Client-side and server-side validation, anti-forgery protection
- Safe delete rules (an author with books cannot be deleted)
- Responsive UI with Bootstrap 5

## Tech stack
C#, ASP.NET Core MVC (.NET 10), Entity Framework Core (Code-First, Migrations), SQL Server LocalDB, LINQ, Razor, Bootstrap 5, jQuery Validation

## Architecture
Controllers → Repository (`IBookstoreRepository<T>`) → EF Core `DbContext`, with ViewModels for forms and Dependency Injection for services.

## Run locally
Requirements: .NET 10 SDK and SQL Server LocalDB (included with Visual Studio).

```bash
git clone https://github.com/zidanalhilali/Bookstore.git
cd Bookstore
dotnet run
```
In Development the database is created automatically and sample data is added. If it is not, run `dotnet ef database update`.

## Roadmap
- Authentication and authorization (ASP.NET Core Identity)
- Unit tests
- Async data access and pagination
- REST API