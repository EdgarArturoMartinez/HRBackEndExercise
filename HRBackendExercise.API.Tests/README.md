# HR Backend Exercise API - Unit Tests

Comprehensive unit tests for the HRBackendExercise.API project using xUnit and Moq.

## Test Coverage

### ProductService Tests
- **Create Tests**: 5 tests covering valid creation, null checks, SKU validation, price validation, and ID generation
- **GetById Tests**: 2 tests for existing and non-existing products
- **GetAll Tests**: 2 tests for empty and populated lists
- **Update Tests**: 5 tests covering updates, validation, and error cases
- **Delete Tests**: 4 tests for deletion scenarios

### ProductsController Tests
- **GetAll Tests**: 3 tests covering success cases, empty lists, and error handling
- **Get Tests**: 3 tests for retrieving products by ID
- **Post Tests**: 6 tests covering creation, validation, and error scenarios
- **Put Tests**: 7 tests for update operations with various validation cases
- **Delete Tests**: 3 tests for deletion with success and error cases

**Total: 40 unit tests**

## Technologies Used

- **xUnit**: Testing framework
- **Moq**: Mocking framework for dependencies
- **.NET 8.0**: Target framework

## Running the Tests

### Run all tests
```bash
dotnet test
```

### Run tests with detailed output
```bash
dotnet test --verbosity detailed
```

### Run tests with code coverage (requires coverage tool)
```bash
dotnet test --collect:"XPlat Code Coverage"
```

## Test Organization

```
HRBackendExercise.API.Tests/
├── Controllers/
│   └── ProductsControllerTests.cs    # API Controller tests with mocking
├── Services/
│   └── ProductServiceTests.cs        # Service layer tests
└── HRBackendExercise.API.Tests.csproj
```

## Test Categories

Each test class is organized into regions by HTTP method or operation:
- Create/Post tests
- Read/Get tests
- Update/Put tests
- Delete tests

## Assertions Covered

- Valid data handling
- Null reference checks
- Empty/whitespace validation
- Numeric boundary conditions
- Exception handling
- HTTP status code verification
- Data integrity after operations
