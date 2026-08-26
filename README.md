# Product & Customer Management System

A lightweight **ASP.NET Core MVC** application for managing an in-memory catalog of **Products**, **Customers**, and **Orders** — built as a learning/demo project to explore MVC + Web API patterns, service-layer abstraction, and simulated relational data (PK/FK relationships) without a real database.

The system exposes both a **traditional server-rendered MVC UI** (tables, forms, edit/delete) and a **RESTful Web API** for each resource, all backed by shared in-memory service classes.

---

## What the System Does

- **Product management**
  - View a tabular list of all products
  - Add a new product via a form
  - Edit and delete existing products
  - Full CRUD available via a REST API as well as the MVC UI

- **Customer management**
  - View a tabular list of all customers
  - Add, edit, and delete customers (MVC forms + REST API)

- **Order management**
  - Create orders that link a **Customer** to one or more **Products**
  - Each order is made up of **OrderItems**, which record product, quantity, and unit price at time of purchase
  - Orders enforce relational integrity in code (an order cannot reference a Customer or Product that doesn't exist)

- **Simulated relational data model**
  - Even without a database, the app models real foreign-key relationships:
    - `Order.CustomerId` → `Customer.Id`
    - `OrderItem.OrderId` → `Order.Id`
    - `OrderItem.ProductId` → `Product.Id`
  - Referential integrity (valid FK references, blocking deletes with dependents, etc.) is enforced manually in the service layer, since in-memory collections don't provide this automatically the way a relational database would.

---

## How It Works

### Architecture

The project follows a standard **service-oriented layered architecture**:

```
Controllers (MVC)  ─┐
                     ├──> Interfaces (IProductService, ICustomerService, IOrderService)
API Controllers     ─┘              │
                                     ▼
                          Concrete Services (ProductService, CustomerService, OrderService)
                                     │
                                     ▼
                          In-Memory Collections (List<T>)
```

- **Models** — Plain C# classes (`Product`, `Customer`, `Order`, `OrderItem`) representing the domain entities and their relationships.
- **Service Interfaces** — `IProductService`, `ICustomerService`, `IOrderService` define the contract for CRUD operations. Controllers depend only on these interfaces, not concrete implementations.
- **Concrete Services** — `ProductService`, `CustomerService`, `OrderService` hold the actual in-memory `List<T>` collections and implement the interface methods (`GetAll`, `GetById`, `Add`, `Update`, `Delete`). Cross-entity validation (e.g. checking a `CustomerId` exists before creating an `Order`) also lives here.
- **Dependency Injection** — Services are registered as **singletons** in `Program.cs`, since the in-memory data must persist across requests for the lifetime of the application:
  ```csharp
  builder.Services.AddSingleton<IProductService, ProductService>();
  builder.Services.AddSingleton<ICustomerService, CustomerService>();
  builder.Services.AddSingleton<IOrderService, OrderService>();
  ```
- **MVC Controllers** — Serve server-rendered Razor views (`Index`, `Create`, `Edit`, `Delete`) for a traditional web UI experience with tables and forms.
- **API Controllers** — Expose the same underlying functionality as JSON over REST endpoints (`GET`, `POST`, `PUT`, `DELETE`) for programmatic or front-end (SPA/mobile) consumption.

### Why an Interface + Service Pattern?

Decoupling controllers from concrete service implementations via interfaces:
- Makes the code easier to unit test (services can be mocked)
- Allows the in-memory implementation to be swapped for a real database-backed implementation (e.g. Entity Framework Core) later, without touching controller code
- Keeps controllers thin — they only orchestrate requests/responses, not business logic

### Simulating PK/FK Relationships In-Memory

Since there's no database engine enforcing constraints, the app replicates that behavior manually:
- Every model has an `Id` property acting as its primary key, auto-incremented by its service.
- Related models store the parent's `Id` as a foreign key field (e.g. `Order.CustomerId`).
- Before inserting a record with a foreign key, the relevant service looks up the referenced entity and validates it exists.
- Deletes that would orphan dependent records (e.g. deleting a Customer with existing Orders) are blocked at the service layer.

---

## Tech Stack

| Layer                | Technology                                                        |
|-----------------------|--------------------------------------------------------------------|
| Framework             | ASP.NET Core MVC (.NET)                                           |
| Language              | C#                                                                 |
| UI                    | Razor Views (`.cshtml`), Bootstrap (default ASP.NET Core styling) |
| API                   | ASP.NET Core Web API (`[ApiController]`)                          |
| Data storage          | In-memory `List<T>` collections (no external database)           |
| Dependency Injection  | Built-in ASP.NET Core DI container                                |
| Architecture          | Service interface / concrete service pattern (Repository-style)  |

> No external database, ORM, or persistence layer is currently used — all data is held in memory and reset when the application restarts.

---

## Project Structure (typical layout)

```
├── Controllers/
│   ├── ProductsController.cs        # MVC controller
│   ├── CustomersController.cs       # MVC controller
│   ├── OrdersController.cs          # MVC controller
│   └── Api/
│       ├── ProductsApiController.cs
│       ├── CustomersApiController.cs
│       └── OrdersApiController.cs
├── Models/
│   ├── Product.cs
│   ├── Customer.cs
│   ├── Order.cs
│   └── OrderItem.cs
├── Services/
│   ├── IProductService.cs / ProductService.cs
│   ├── ICustomerService.cs / CustomerService.cs
│   └── IOrderService.cs / OrderService.cs
├── Views/
│   ├── Products/
│   ├── Customers/
│   └── Orders/
└── Program.cs
```

---

## Getting Started

### Prerequisites
- [.NET 8 SDK](https://dotnet.microsoft.com/download) or later

### Run the project
```bash
git clone https://github.com/DylanKemper/CommerceSystem.git
cd Stockify
dotnet restore
dotnet run --project Stockify/Stockify.csproj
```

Then navigate to:
- `https://localhost:<port>/Products` — Product management UI
- `https://localhost:<port>/Customers` — Customer management UI
- `https://localhost:<port>/Orders` — Order management UI
- `https://localhost:<port>/api/products` — Products REST API
- `https://localhost:<port>/api/customers` — Customers REST API
- `https://localhost:<port>/api/orders` — Orders REST API

---

## Future Expansions

- **Persistent storage** — Replace in-memory collections with a real database using Entity Framework Core (SQL Server, PostgreSQL, or SQLite), while keeping the existing service interfaces unchanged.
- **Validation & error handling** — Add model validation attributes, custom exception types, and consistent API error responses (e.g. `ProblemDetails`).
- **Authentication & authorization** — Add user accounts/roles (e.g. Admin vs. Staff) to restrict who can add/edit/delete records.
- **Order workflow** — Add order statuses (Pending, Shipped, Cancelled, Completed) and status transition logic.
- **Search, filtering & pagination** — For Products, Customers, and Orders tables as data volume grows.
- **Unit & integration tests** — Test coverage for service logic (especially FK validation rules) using xUnit/NUnit and mocked interfaces.
- **Front-end enhancements** — AJAX-based edit/delete (no full page reloads), client-side validation, and a richer "create order" screen supporting multi-product selection with running totals.
- **Reporting/dashboard** — Sales summaries, top customers, top products, revenue over time.
- **API documentation** — Swagger/OpenAPI integration for the REST endpoints.
- **Containerization** — Dockerfile for consistent deployment across environments.

---

## License

*MIT*
