# Client Dossier API

A .NET Core Web API for managing client information, tasks, documents, and invoices.

## Features

- JWT-based authentication
- Client management (CRUD operations)
- Task management for clients
- Document upload and management
- Invoice management
- User-specific data access control

## Prerequisites

- .NET 8.0 SDK
- SQL Server (LocalDB or full instance)
- Visual Studio 2022 or later

## Getting Started

1. Clone the repository
2. Update the connection string in `appsettings.json` if needed
3. Open the solution in Visual Studio
4. Run the following commands in the Package Manager Console:
   ```
   Add-Migration InitialCreate
   Update-Database
   ```
5. Run the application

## API Endpoints

### Authentication
- POST /api/auth/register - Register a new user
- POST /api/auth/login - Login and get JWT token

### Clients
- GET /api/clients - Get all clients for the current user
- POST /api/clients - Create a new client
- PUT /api/clients/{id} - Update a client
- DELETE /api/clients/{id} - Delete a client

### Tasks
- GET /api/clients/{clientId}/tasks - Get all tasks for a client
- POST /api/clients/{clientId}/tasks - Create a new task for a client

### Documents
- GET /api/clients/{clientId}/documents - Get all documents for a client
- POST /api/clients/{clientId}/documents - Upload a new document for a client

### Invoices
- GET /api/clients/{clientId}/invoices - Get all invoices for a client
- POST /api/clients/{clientId}/invoices - Create a new invoice for a client

## Security

- All endpoints except registration and login require JWT authentication
- Each user can only access their own data
- Passwords are hashed using SHA256
- JWT tokens expire after 7 days

## File Storage

- Documents are stored in the `wwwroot/uploads/{clientId}` directory
- Each file is stored with a unique name to prevent conflicts

## Development

The project uses:
- Entity Framework Core 8.0 for data access
- JWT Bearer authentication
- Swagger/OpenAPI for API documentation
- SQL Server for data storage 