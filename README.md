# eCommerce Web Service API

An ASP.NET Core Web API for managing an eCommerce platform, offering endpoints for products, categories, orders, users, and more. The API includes authentication and authorization to differentiate between admin and regular user roles.

## Features

- ?? **User Management**: Register, login, and manage users with role-based access control (User/Admin).
- ?? **Product Management**: Create, update, delete, and fetch product information.
- ?? **Category Management**: Organize products into categories.
- ?? **Order Management**: Place and track orders.
- ?? **Authentication and Authorization**: JWT-based authentication with role-based authorization for admin and user actions.

## Technologies Used

- **ASP.NET Core 6** - Web API Framework ![ASP.NET](https://img.shields.io/badge/ASP.NET%20Core-512BD4?logo=dotnet&logoColor=white)
- **Entity Framework Core** - ORM for database interactions ![EF Core](https://img.shields.io/badge/Entity%20Framework%20Core-7A3B9A?logo=dotnet&logoColor=white)
- **SQL Server** - Database ![SQL Server](https://img.shields.io/badge/SQL%20Server-CC2927?logo=microsoftsqlserver&logoColor=white)
- **JWT (JSON Web Token)** - For secure authentication ![JWT](https://img.shields.io/badge/JWT-000000?logo=json-web-tokens&logoColor=white)

## API Endpoints

### Auth

| **Endpoint**           | **Method**                              | **Description**                                    | **Role**    |
|------------------------|-----------------------------------------|----------------------------------------------------|-------------|
| /Auth/register         | <span style="color:yellow">POST</span>  | Register a new user                                | User/Admin  |
| /Auth/login			 | <span style="color:yellow">POST</span>  | Authenticate a user and return a JWT token         | User/Admin  |

### Role

| **Endpoint**                       | **Method**                               | **Description**                                     | **Role**    |
|------------------------------------|------------------------------------------|-----------------------------------------------------|-------------|
| /Role/add-role                     | <span style="color:yellow">POST</span>   | Adding a new role                                   | Admin       |
| /Role/assigning-role	             | <span style="color:yellow">POST</span>   | Assigning role to user                              | Admin       |
| /Role/update-role                  | <span style="color:yellow">POST</span>   | Updating a role                                     | Admin       |
| /Role/getcreated-roles/:email	     | <span style="color:green">GET</span>     | Getting all created Roles by email                  | Admin       |
| /Role/delete-role/:roleName/:email | <span style="color:red">DELETE</span>    | Delete role base on name and email                  | Admin       |
| /Role/getAssigned-roles/:email	 | <span style="color:green">GET</span>     | Getting all Assigned Roles by email                 | User/Admin  |  


## Getting Started

### Prerequisites

- [.NET 6 SDK](https://dotnet.microsoft.com/download/dotnet/6.0) ![.NET](https://img.shields.io/badge/.NET-5C2D8A?logo=.net&logoColor=white)
- [SQL Server](https://www.microsoft.com/en-us/sql-server/sql-server-downloads) or a compatible database ![SQL Server](https://img.shields.io/badge/SQL%20Server-CC2927?logo=microsoftsqlserver&logoColor=white)
- Optional: [Postman](https://www.postman.com/) for testing API endpoints ![Postman](https://img.shields.io/badge/Postman-FF6C37?logo=postman&logoColor=white)

### Installation

1. **Clone the repository** ![Git](https://img.shields.io/badge/Git-F05032?logo=git&logoColor=white)

   ```bash
   git clone https://github.com/kirtanp04/Ecommerce-Webservice-API.git
   cd ecommerce-webservice-api
