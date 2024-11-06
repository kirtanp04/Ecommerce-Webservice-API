# eCommerce Web Service API

An ASP.NET Core Web API for managing an eCommerce platform, offering endpoints for products, categories, orders, users, and more. The API includes authentication and authorization to differentiate between admin and regular user roles.

## Features

- 👤 **User Management**: Register, login, and manage users with role-based access control (User/Admin).
- 🛒 **Product Management**: Create, update, delete, and fetch product information.
- 📦 **Category Management**: Organize products into categories.
- 📋 **Order Management**: Place and track orders.
- 🔒 **Authentication and Authorization**: JWT-based authentication with role-based authorization for admin and user actions.

## Technologies Used

- ![ASP.NET](https://img.shields.io/badge/ASP.NET%20Core-512BD4?logo=dotnet&logoColor=white) - Web API Framework 
- ![EF Core](https://img.shields.io/badge/Entity%20Framework%20Core-7A3B9A?logo=dotnet&logoColor=white) - ORM for database interactions 
- ![SQL Server](https://img.shields.io/badge/SQL%20Server-CC2927?logo=microsoftsqlserver&logoColor=white) - Database 
- ![JWT](https://img.shields.io/badge/JWT-000000?logo=json-web-tokens&logoColor=white) - For secure authentication 

## API Endpoints

### Auth

| **Endpoint**           | **Method** | **Description**                                    | **Role**    |
|------------------------|------------|----------------------------------------------------|-------------|
| /Auth/register         | POST       | Register a new user                                | User/Admin  |
| /Auth/login			 | POST       | Authenticate a user and return a JWT token         | User/Admin  |

### Role

| **Endpoint**                       | **Method**   | **Description**                          | **Role**    |
|------------------------------------|--------------|------------------------------------------|-------------|
| /Role/add-role                     | POST         | Adding a new role                        | Admin       |
| /Role/assigning-role	             | POST         | Assigning role to user                   | Admin       |
| /Role/update-role                  | POST         | Updating a role                          | Admin       |
| /Role/getcreated-roles/:email	     | GET          | Getting all created Roles by email       | Admin       |
| /Role/delete-role/:roleName/:email | DELETE       | Delete role base on name and email       | Admin       |
| /Role/getAssigned-roles/:email	 | GET          | Getting all Assigned Roles by email      | User/Admin  |  

### Categories

| **Endpoint**                       | **Method**   | **Description**                          | **Role**    |
|------------------------------------|--------------|------------------------------------------|-------------|
| /Category/add                      | POST         | Adding a new Category                    | Admin       |
| /Category/getall	                 | GET          | Getting categories by user               | Admin       |
| /Category/update                   | POST         | Updating a Category                      | Admin       |
| /Category/delete?Id	             | DELETE       | Deleting category                        | Admin       |

### Product

| **Endpoint**                          | **Method**   | **Description**                              | **Role**    |
|---------------------------------------|--------------|----------------------------------------------|-------------|
| /Product/add                          | POST         | Adding a new Product                         | Admin       |
| /Product/active-inactive/:productId	| POST         | Making product active/Inactive for shopping  | Admin       |
| /Product/update                       | POST         | Updating a Product                           | Admin       |
| /Product/delete?productId	            | DELETE       | Deleting a Product                           | Admin       |
| /Product/getall-product				| GET		   | Getting all Products                         | User        |
	


## Getting Started

### Prerequisites

- ![.NET](https://img.shields.io/badge/.NET-5C2D8A?logo=.net&logoColor=white)(https://dotnet.microsoft.com/download/dotnet/6.0) 
- ![SQL Server](https://img.shields.io/badge/SQL%20Server-CC2927?logo=microsoftsqlserver&logoColor=white)(https://www.microsoft.com/en-us/sql-server/sql-server-downloads) or a compatible database 
- Optional: ![Postman](https://img.shields.io/badge/Postman-FF6C37?logo=postman&logoColor=white)(https://www.postman.com/) for testing API endpoints 

### Installation

1. **Clone the repository** ![Git](https://img.shields.io/badge/Git-F05032?logo=git&logoColor=white)

   ```bash
   git clone https://github.com/kirtanp04/Ecommerce-Webservice-API.git
   cd ecommerce-webservice-api
