# HR Backend Exercise API - Technical Interview Guide

## Architecture Overview

This is a RESTful API built with ASP.NET Core 8.0 following **Clean Architecture** and **SOLID principles**. I designed it to demonstrate production-ready patterns while keeping it simple enough to explain and maintain.

**Real-Life Analogy**: Think of this API like a restaurant:
- **Controllers** = Waiters (take orders from customers)
- **Services** = Kitchen (prepare the food/logic)
- **Models** = Menu Items (what we're serving)
- **Abstractions/Interfaces** = Recipe Book (standards everyone follows)

---

## 🏗️ Project Structure - WHY Each Folder Exists

```
HRBackendExercise.API/
├── Abstractions/          # Interfaces - The "Contract"
│   └── IProductService.cs
├── Controllers/           # API Layer - Entry Point
│   └── ProductsController.cs
├── Models/               # Data Transfer Objects (DTOs)
│   └── Product.cs
├── Services/             # Business Logic Layer
│   └── ProductService.cs
├── Program.cs            # Dependency Injection & Configuration
└── appsettings.json      # External Configuration
```

### Why This Structure?

**Interviewer might ask**: "Why separate folders?"

**Your Answer**: "I followed the **Separation of Concerns** principle. Each layer has ONE responsibility:
- **Controllers**: Handle HTTP requests/responses (routing, status codes)
- **Services**: Contain business rules (validation, data manipulation)
- **Models**: Define data structure
- **Abstractions**: Enable dependency injection and testability"

---

## 📁 Deep Dive: Each File Explained

### 1️⃣ Program.cs - The Heart of DI

```csharp
builder.Services.AddSingleton<IProductService, ProductService>();
```

**Why AddSingleton instead of AddScoped or AddTransient?**

| Lifetime | Created | Destroyed | Best For |
|----------|---------|-----------|----------|
| **Singleton** | Once per app | When app stops | Stateful services, shared data |
| **Scoped** | Once per request | End of request | Database contexts, request-specific |
| **Transient** | Every time requested | ASAP | Lightweight, stateless services |

**My Reasoning**: I used **AddSingleton** because:
1. ✅ The service uses an **in-memory list** that must persist across requests
2. ✅ All users need to see the same product catalog
3. ✅ Better performance (created once, reused)

**Real-Life Analogy**: 
- **Singleton** = Restaurant menu board (one for everyone)
- **Scoped** = Table number (unique per dining session)
- **Transient** = Napkins (grab a new one each time)

**Code Example to Explain**:
```csharp
// If we used Scoped, each request would have its own empty list!
// Request 1: POST /products → Creates product with ID 1
// Request 2: GET /products → Returns empty [] (different list instance!)

// With Singleton:
// Request 1: POST /products → Creates product with ID 1
// Request 2: GET /products → Returns [Product ID 1] ✅
```

---

### 2️⃣ Models/Product.cs - Simple POCO

```csharp
public class Product
{
    public int Id { get; set; }
    public string? Description { get; set; }
    public string? SKU { get; set; }  // Stock Keeping Unit
    public decimal Price { get; set; }
}
```

**Why This Design?**
- ✅ **Nullable properties** (`string?`) prevent runtime null exceptions
- ✅ **Decimal for Price** (not double/float) - financial precision matters
- ✅ **Auto-properties** - clean, simple syntax

**Real-Life Analogy**: This is like a product label in a warehouse - it has all the info you need to identify and price an item.

---

### 3️⃣ Abstractions/IProductService.cs - The Contract

```csharp
public interface IProductService
{
    Product Create(Product entity);
    Product? GetById(int id);
    IEnumerable<Product> GetAll();
    void Update(Product entity);
    void Delete(Product entity);
}
```

**Why Use an Interface?**

**Interviewer**: "Couldn't you just use ProductService directly?"

**Your Answer**: "Using interfaces provides three critical benefits:

1. **Dependency Inversion** (SOLID): High-level modules (Controllers) don't depend on low-level modules (Services)
2. **Testability**: I can inject a mock service in tests without touching the database
3. **Flexibility**: Tomorrow I could swap in-memory storage for SQL without changing the controller"

**Simple Example**:
```csharp
// Without Interface - TIGHT COUPLING ❌
public ProductsController(ProductService service) { }

// With Interface - LOOSE COUPLING ✅
public ProductsController(IProductService service) { }
// Now tests can inject: Mock<IProductService>
```

---

### 4️⃣ Services/ProductService.cs - Business Logic

```csharp
private readonly List<Product> _products = new List<Product>();
private int _nextId = 1;

public Product Create(Product entity)
{
    if (entity == null)
        throw new ArgumentNullException(nameof(entity));
    
    if (string.IsNullOrWhiteSpace(entity.SKU))
        throw new ArgumentException("SKU cannot be null or empty.");
    
    if (entity.Price <= 0)
        throw new ArgumentException("Price must be greater than 0.");
    
    entity.Id = _nextId++;
    _products.Add(entity);
    return entity;
}
```

**Design Decisions Explained**:

1. **Private Fields** (`_products`, `_nextId`):
   - Encapsulation - external code can't corrupt the list
   - **Real-Life**: Like a chef's recipe notes - customers don't access them directly

2. **Guard Clauses** (the `if` checks at the top):
   - Fail fast if input is invalid
   - **Real-Life**: Like TSA checking your ticket before you board
   - **Alternative**: Could use FluentValidation, but guard clauses are simpler for small APIs

3. **Manual ID Generation** (`_nextId++`):
   - Simulates database auto-increment
   - Thread-safe for single instance (Singleton!)

**Interview Example**:
```csharp
// Why throw exceptions in Service but return BadRequest in Controller?

// SERVICE: Throws exceptions (business rule violation)
if (entity.Price <= 0)
    throw new ArgumentException("Price must be greater than 0.");

// CONTROLLER: Catches and translates to HTTP status codes
catch (ArgumentException ex)
{
    return BadRequest(new { message = ex.Message });
}
```

**Why Separate?** 
- Services don't know about HTTP (reusable in console apps, background jobs)
- Controllers handle HTTP-specific concerns (status codes, headers)

---

### 5️⃣ Controllers/ProductsController.cs - The API Layer

```csharp
[Route("api/[controller]")]
[ApiController]
public class ProductsController : ControllerBase
{
    private readonly IProductService _productsService;
    
    public ProductsController(IProductService productsService)
    {
        _productsService = productsService;
    }
```

**Key Patterns**:

1. **Dependency Injection via Constructor**:
```csharp
public ProductsController(IProductService productsService)
{
    _productsService = productsService;
}
```
**Why not this?**
```csharp
var service = new ProductService(); // ❌ Tight coupling, can't test
```

2. **Try-Catch Blocks**:
```csharp
try
{
    var products = _productsService.GetAll();
    return Ok(products);
}
catch (Exception ex)
{
    return StatusCode(500, new { message = $"An error occurred..." });
}
```
**Why**: Even with validation, unexpected errors can occur (out of memory, etc.)

3. **HTTP Status Codes**:
```csharp
return Ok(products);              // 200 - Success
return Created(...);               // 201 - Resource created
return NoContent();                // 204 - Success with no body
return BadRequest(...);            // 400 - Client error
return NotFound(...);              // 404 - Resource not found
return StatusCode(500, ...);       // 500 - Server error
```

**Real-Life Analogy**: 
- **200 OK** = "Here's your order!"
- **201 Created** = "Order placed, here's your receipt #123"
- **400 Bad Request** = "Sorry, we don't serve that"
- **404 Not Found** = "That menu item doesn't exist"
- **500 Error** = "Kitchen on fire, please wait"

---

## 🧪 Testing Strategy - 56 Tests, 100% Coverage

### Unit vs Integration Tests

**I chose Unit Tests because**:
1. ✅ Fast execution (milliseconds)
2. ✅ Isolated - one test per method
3. ✅ Easy to debug failures

### Example Test Pattern (AAA):

```csharp
[Fact]
public void Create_WithValidProduct_ReturnsProductWithId()
{
    // ARRANGE - Set up test data
    var service = new ProductService();
    var product = new Product { SKU = "PROD-001", Price = 99.99m };
    
    // ACT - Execute the method
    var result = service.Create(product);
    
    // ASSERT - Verify the outcome
    Assert.NotNull(result);
    Assert.Equal(1, result.Id);
}
```

**Why Moq in Controller Tests?**
```csharp
var mockService = new Mock<IProductService>();
mockService.Setup(s => s.GetAll()).Returns(products);
```
**Reason**: Tests the controller in isolation without running the actual service

---

## 🎯 Common Interview Questions - Be Ready!

### Q1: "Why not use a database?"
**A**: "For this exercise, in-memory storage demonstrates the architecture without infrastructure complexity. In production, I'd swap the implementation to use Entity Framework with SQL Server - the interface makes this trivial."

### Q2: "What if two requests try to create products at the same time?"
**A**: "Current implementation isn't thread-safe. In production, I'd use:
```csharp
private static readonly object _lock = new object();
public Product Create(Product entity)
{
    lock(_lock) 
    {
        entity.Id = _nextId++;
        _products.Add(entity);
    }
    return entity;
}
```
Or use `ConcurrentDictionary` for better performance."

### Q3: "Why validation in both Controller AND Service?"
**A**: 
- **Controller**: HTTP-level validation (null checks, basic format)
- **Service**: Business-rule validation (duplicate SKU, price ranges)
- **Defense in Depth**: Service can be called from anywhere, not just controllers

### Q4: "How would you add authentication?"
**A**: "I'd use JWT tokens:
```csharp
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(...);
    
[Authorize] // Add to controller
public class ProductsController : ControllerBase
```

---

## 🚀 Running the Application

```bash
dotnet build
dotnet run
# Navigate to https://localhost:5001/swagger
```

## 🧪 Running Tests

```bash
cd HRBackendExercise.API.Tests
dotnet test --verbosity detailed
```

---

## 📚 Technologies & Why I Chose Them

| Technology | Purpose | Why? |
|------------|---------|------|
| **ASP.NET Core 8.0** | Framework | Latest LTS, cross-platform |
| **Swagger/OpenAPI** | API Docs | Auto-generated, interactive |
| **xUnit** | Test Framework | .NET standard, clean syntax |
| **Moq** | Mocking | Easy interface mocking |
| **C# 12** | Language | Nullable reference types |

---

## 🎓 Key Takeaways for Interview

1. **Clean Architecture**: Separation of concerns (Controller → Service → Model)
2. **SOLID Principles**: Especially Dependency Inversion (interfaces)
3. **Singleton Pattern**: Chosen deliberately for shared state
4. **Guard Clauses**: Fail-fast validation
5. **HTTP Standards**: Proper status codes and REST conventions
6. **Testability**: 100% coverage through dependency injection

**Final Tip**: Walk through a request lifecycle:
1. HTTP POST hits `ProductsController.Post()`
2. Controller validates input, calls `_productService.Create()`
3. Service validates business rules, adds to in-memory list
4. Controller returns `201 Created` with location header
5. Next GET request sees the new product (because Singleton!)

Good luck with your interview! 🚀
