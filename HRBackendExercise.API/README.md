# HR Backend Exercise API

A RESTful API built with ASP.NET Core 8.0 for managing products.

## Features

- **CRUD Operations**: Create, Read, Update, and Delete products
- **In-Memory Storage**: Uses an in-memory list for data storage
- **Swagger Documentation**: Interactive API documentation
- **Input Validation**: Comprehensive validation for all endpoints
- **Error Handling**: Proper error responses with meaningful messages
- **Unit Tests**: 56 comprehensive unit tests with xUnit and Moq (100% coverage)

## Project Structure

```
HRBackendExercise.API/
├── Abstractions/          # Interfaces
│   └── IProductService.cs
├── Controllers/           # API Controllers
│   └── ProductsController.cs
├── Models/               # Data models
│   └── Product.cs
├── Services/             # Business logic
│   └── ProductService.cs
├── Program.cs            # Application entry point
└── appsettings.json      # Configuration
```

## Prerequisites

- .NET 8.0 SDK or later

## Getting Started

### Clone the repository

```bash
git clone https://github.com/EdgarArturoMartinez/HRBackendExercise.API.git
cd HRBackendExercise.API
```

### Build the project

```bash
dotnet build
```

### Run the application

```bash
dotnet run
```

The API will be available at:
- HTTP: `http://localhost:5000`
- HTTPS: `https://localhost:5001`

### Access Swagger UI

Open your browser and navigate to:
```
https://localhost:5001/swagger
```

## API Endpoints

### Products

| Method | Endpoint | Description |
|--------|----------|-------------|
| GET | `/api/products` | Get all products |
| GET | `/api/products/{id}` | Get product by ID |
| POST | `/api/products` | Create a new product |
| PUT | `/api/products/{id}` | Update an existing product |
| DELETE | `/api/products/{id}` | Delete a product |

### Example Request Body (POST/PUT)

```json
{
  "sku": "PROD-001",
  "description": "Sample Product",
  "price": 99.99
}
```

## Product Model

```csharp
{
  "id": 1,
  "sku": "PROD-001",
  "description": "Sample Product",
  "price": 99.99
}
```

## Validation Rules

- **SKU**: Required, cannot be empty
- **Price**: Must be greater than 0
- **Description**: Optional

## Unit Tests

The project includes comprehensive unit tests located in `HRBackendExercise.API.Tests`:

- **56 total tests** covering all functionality
- **ProductService Tests**: 21 tests for business logic
- **ProductsController Tests**: 35 tests for API endpoints
- **100% code coverage** of service and controller layers

### Running Tests

```bash
cd HRBackendExercise.API.Tests
dotnet test
```

For detailed test output:
```bash
dotnet test --verbosity detailed
```

## Technologies Used

- ASP.NET Core 8.0
- Swagger/OpenAPI
- C# 12
- xUnit (Testing Framework)
- Moq (Mocking Library)

## License

This project is created for educational purposes.
