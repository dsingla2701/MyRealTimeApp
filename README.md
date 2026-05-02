# Real-Time Bi-Directional GraphQL API (.NET 10)

A robust proof-of-concept demonstrating **real-time, bi-directional data synchronization** between a SQL Server database and a web frontend. Built on **.NET 10** using **Clean Architecture**, this API leverages **GraphQL (HotChocolate)** for all CRUD operations and WebSockets for event-driven database listening.

## Key Features

* **Pure GraphQL Implementation:** Queries, Mutations, and Subscriptions are unified under a single `/graphql` endpoint.
* **Bi-Directional Synchronization:** 
  * **API ➔ DB:** Application mutations update the database and broadcast to connected clients.
  * **DB ➔ API:** Direct database modifications (e.g., via SSMS) are intercepted and broadcasted to the frontend instantly via WebSockets.
* **Clean Architecture:** Strict decoupling of Domain, Application, Infrastructure, and API layers.
* **Event-Driven Database Listening:** Utilizes `SqlTableDependency` and the SQL Server Service Broker to capture row-level changes without resource-heavy polling.

## Tech Stack

* **Framework:** .NET 10
* **Language:** C# 14
* **GraphQL Engine:** HotChocolate 14
* **ORM:** Entity Framework Core 10
* **Database:** SQL Server (LocalDB)
* **Real-Time Engine:** WebSockets & SqlTableDependency

## Architecture Design

The solution enforces Dependency Inversion, pointing all dependencies inward toward the Domain layer:

| **1. Core.Domain** | Enterprise logic & core models. | `Product.cs` (No external dependencies). |
| **2. Core.Application** | Business rules & use cases. | `IProductService`, `IProductRepository`, `ProductService.cs`. |
| **3. Infrastructure** | External concerns (DB, Network). | `AppDbContext`, `ProductRepository`, `SqlListener`. |
| **4. API** | Entry point & routing. | GraphQL Setup (`Query`, `Mutation`, `Subscription`), `Program.cs`. |


### Prerequisites
* .NET 10 SDK
* SQL Server or LocalDB
* SQL Server Management Studio (SSMS)

### 1. Database Configuration
Enable the Service Broker on your SQL Server instance to allow event listening. Open SSMS and execute:

```sql
ALTER DATABASE [MyRealTimeDB] SET ENABLE_BROKER WITH ROLLBACK IMMEDIATE;
```

### 2. Connection String 
Ensure the appsettings.json in the root directory points to your database:

### 3. Initialize Database (EF Core Migrations)
Open your terminal in the project root and apply the migrations to generate the schema:
dotnet tool install --global dotnet-ef
dotnet ef migrations add InitialCreate
dotnet ef database update

### 4. Run the API
Start the development server : dotnet run


**To test the bi-directional flow**
Use the built-in Banana Cake Pop IDE to verify real-time capabilities.

Step 1: Initialize the Listener
Open tab 1 and execute this Subscription. Leave it running to keep the WebSocket open.
```
subscription {
  onProductUpdated {
    id
    name
    price
  }
}
```

Step 2: Trigger API ➔ DB Flow
Open Tab 2 and execute a Mutation to insert a product. Observe Tab 1; the new product will be pushed instantly.
```
mutation {
  addProduct(name: "Mechanical Keyboard", price: 89.99) {
    id
    name
  }
}
```

Step 3: Trigger DB ➔ API Flow
Leave the Subscription running. Open SSMS and execute a direct SQL update:

UPDATE [dbo].[Products] 
SET Price = 75.00 
WHERE Name = 'Mechanical Keyboard';

Step 4: To fetch the data stored in DB
```
query {
  products {
    id
    name
    price
  }
}
```

Return to your browser; the Subscription in Tab 1 will have intercepted the database change and logged the new price.
