# HR Backend Exercise

A RESTful API built with ASP.NET Core 8.0 for managing products, with comprehensive unit tests.

## 🚀 Features

- **CRUD Operations**: Create, Read, Update, and Delete products
- **In-Memory Storage**: Uses an in-memory list for data storage
- **Swagger Documentation**: Interactive API documentation
- **Input Validation**: Comprehensive validation for all endpoints
- **Error Handling**: Proper error responses with meaningful messages
- **Unit Tests**: Comprehensive unit tests with xUnit and Moq

## 📁 Project Structure

```
HRBackEndExercise/
├── HRBackendExercise.API/          # Main API Project
│   ├── Abstractions/               # Interfaces
│   ├── Controllers/                # API Controllers
│   ├── Models/                     # Data models
│   ├── Services/                   # Business logic
│   └── Program.cs                  # Application entry point
├── HRBackendExercise.API.Tests/    # Unit Tests Project
│   ├── Controllers/                # Controller tests
│   └── Services/                   # Service tests
└── HRBackendExercise.sln           # Solution file
```

## 🛠 Prerequisites

- .NET 8.0 SDK or later
- Visual Studio 2022 or VS Code (optional)

## 🏃 Getting Started

### 1. Clone the repository

```bash
git clone https://github.com/EdgarArturoMartinez/HRBackEndExercise.git
cd HRBackEndExercise
```

### 2. Restore dependencies

```bash
dotnet restore
```

### 3. Build the project

```bash
dotnet build
```

### 4. Run the application

```bash
cd HRBackendExercise.API
dotnet run
```

The API will be available at:
- HTTP: `http://localhost:5000`
- HTTPS: `https://localhost:5001`
- Swagger UI: `https://localhost:5001/swagger`

## 🧪 Running Tests

Run all tests:

```bash
dotnet test
```

Run tests with coverage:

```bash
dotnet test /p:CollectCoverage=true
```

## 📝 API Endpoints

### Products

- `GET /api/products` - Get all products
- `GET /api/products/{id}` - Get a product by ID
- `POST /api/products` - Create a new product
- `PUT /api/products/{id}` - Update a product
- `DELETE /api/products/{id}` - Delete a product

### Sample Request

**Create Product:**
```json
POST /api/products
{
  "name": "Laptop",
  "description": "High-performance laptop",
  "price": 1299.99
}
```

**Response:**
```json
{
  "id": 1,
  "name": "Laptop",
  "description": "High-performance laptop",
  "price": 1299.99
}
```

## 🏗 Technologies Used

- **ASP.NET Core 8.0** - Web API framework
- **Swagger/OpenAPI** - API documentation
- **xUnit** - Testing framework
- **Moq** - Mocking library for unit tests

## 📄 License

This project is created as a coding exercise.

## 👤 Author

Edgar Arturo Martinez
- GitHub: [@EdgarArturoMartinez](https://github.com/EdgarArturoMartinez)
