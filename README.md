# MarketplaceAPI

The goal of the Marketplace API project is to create a software that can generate a database on SQL Server and can serve the users with endpoints to interact with the database. It is a reusable backend solution for any type of online store that includes managing users, orders and products.
 
## Database Generation

- Generates ASP .Net Core Identity tables that handle the users.
- Generates UserActions table that tracks the users actions when interacting with the API.
- Generates Orders table that stores user orders.
- Generates Products table that stores products.
- Generates OrderItems table that stores orders alongside the products ordered.
- Generates Reviews table that stores user reviews on the products.

## REST API Endpoints

The API is still work in progress and currently includes:

- /api/Auth endpoint used for user authentication and authorization.
- /api/Users endpoint that performs different methods on an authenticated user.
- /api/AdminUsers endpoint that allows an admin to perform different methods on users.

Once ready, the API will have a sepparate documentation on each endpoint and each method it uses.

## Testing

The API will be unit tested using a sepparate database that mimics the structure of the main database.
The endpoints are also tested using Postman.

## Technical stack

The programming language used is C# with .Net Core Web API project structure.
The database is created on MS SQL Server.
The project also interacts with different libraries, such as Microsoft.EntityFrameworkCore.SqlServer and Microsoft.AspNetCore.Identity.EntityFrameworkCore for database management, and Microsoft.AspNetCore.Authentication.JwtBearer for authentication and authorization.