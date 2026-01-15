# 🏢 HR Backend Exercise - Product Management System

## 🎯 What Is This? (For Non-Programmers)

Imagine you own a **bookstore** 📚. This application is like having a **digital assistant** that helps you:
- Add new books to your inventory
- Look up book details
- Update book prices
- Remove books from your catalog

But instead of books, we're managing **products**. This is a **web service** (API) that other applications can talk to over the internet.

---

## 🏗️ The Big Picture - Building Architecture Analogy

Think of this project like a **shopping mall**:

```
🏢 THE SHOPPING MALL (HRBackendExercise)
│
├── 🏬 MAIN STORE (HRBackendExercise.API)
│   ├── 📋 Store Policies (Abstractions)
│   ├── 🎫 Ticket Counter (Controllers)
│   ├── 📦 Product Catalog (Models)
│   ├── 👔 Store Manager (Services)
│   └── 🚪 Main Entrance (Program.cs)
│
├── 🔍 Quality Inspector (HRBackendExercise.API.Tests)
│   ├── 🎫 Counter Inspection (Controllers tests)
│   └── 👔 Manager Review (Services tests)
│
└── 📜 Building Blueprint (HRBackendExercise.sln)
```

---

## 📂 Detailed Tour of Each Folder & File

### 🏬 **HRBackendExercise.API** - The Main Store

This is where all the action happens. Like the actual store where customers shop.

#### 📁 **Abstractions/** - Store Policies & Contracts

**Real-Life Analogy**: 📋 **Company Policy Manual**

Just like a company has a manual that says "All managers must be able to: hire, fire, train employees," the Abstractions folder contains **contracts** (interfaces) that say what services MUST do.

**File: `IProductService.cs`**
```
Think of it as: "Job Description for Product Manager"
Must be able to:
- ✅ Create new products
- ✅ Find products by ID
- ✅ Show all products
- ✅ Update product info
- ✅ Delete products
```

**Why it matters**: Anyone who becomes a "Product Manager" (implements this interface) MUST follow these rules.

---

#### 📁 **Controllers/** - The Ticket Counter

**Real-Life Analogy**: 🎫 **Customer Service Desk at the Store**

When you walk into a store and ask "Do you have this product?", you talk to the **person at the counter** (Controller). They don't go to the warehouse themselves—they ask the warehouse manager (Service).

**File: `ProductsController.cs`**
```
Handles customer requests like:
- Customer: "Show me all products" → Controller: "Here's the list!"
- Customer: "I want product #5" → Controller: "Here it is!"
- Customer: "Add this new product" → Controller: "Done! Here's your receipt"
- Customer: "Update product #3" → Controller: "Updated!"
- Customer: "Delete product #7" → Controller: "Removed!"
```

**The Controller's Job**:
1. 🎧 Listen to customer requests (HTTP requests)
2. 🔍 Validate the request ("Do you have all the info I need?")
3. 📞 Call the manager (Service) to do the actual work
4. 📋 Give the customer a proper response (200 OK, 404 Not Found, etc.)

**Why Controllers DON'T do the actual work**: Just like the counter person doesn't go to the warehouse—they're specialists in customer communication, not warehouse management.

---

#### 📁 **Models/** - Product Catalog Templates

**Real-Life Analogy**: 📦 **Product Label Template**

Imagine every product in your store needs a label with specific fields:
- Barcode number
- Description
- SKU (Stock Keeping Unit)
- Price tag

**File: `Product.cs`**
```csharp
public class Product
{
    public int Id { get; set; }              // 🏷️ Unique barcode number
    public string? Description { get; set; } // 📝 "Blue cotton t-shirt"
    public string? SKU { get; set; }         // 🔢 "SHIRT-BLU-001"
    public decimal Price { get; set; }       // 💵 $19.99
}
```

**What it is**: A blueprint/template that defines what information every product must have. Like a form you fill out for each product.

---

#### 📁 **Services/** - The Store Manager

**Real-Life Analogy**: 👔 **Warehouse Manager / Store Manager**

The manager knows where everything is stored, how to add new items, how to remove old stock, and keeps everything organized.

**File: `ProductService.cs`**
```
The Manager's Responsibilities:
- 📦 Maintains the product inventory (in-memory list)
- 🆔 Assigns unique ID numbers to new products
- ✅ Checks if products are valid (price > 0, has SKU, etc.)
- 📍 Finds products by ID
- 📋 Shows you the complete inventory
- ✏️ Updates product information
- 🗑️ Removes products from inventory
```

**How it works**: 
- Has a notebook (List<Product>) where all products are written down
- When someone wants to add a product, the manager writes it down
- When someone asks "What's product #5?", the manager looks it up

**Why it's important**: The Controller (counter person) doesn't know how products are stored. The Service (manager) is the expert.

---

#### 📄 **Program.cs** - The Main Entrance & Setup

**Real-Life Analogy**: 🚪 **Grand Opening Day Setup**

Before opening the store, you need to:
1. Unlock the doors
2. Turn on the lights
3. Hire staff (register services)
4. Set up the cash register
5. Put up the "OPEN" sign

**What `Program.cs` does**:
```csharp
// Morning Setup Checklist:
✅ Create the store (WebApplication)
✅ Hire the Product Manager (Register ProductService)
✅ Set up the counter (Add Controllers)
✅ Install the customer help screen (Swagger documentation)
✅ Open the doors (Start listening for requests)
```

**Line 12 - The Key Line**:
```csharp
builder.Services.AddSingleton<IProductService, ProductService>();
```
**Translation**: "We're hiring ONE store manager (Singleton) for the entire day. Everyone who needs product info talks to the SAME manager."

---

#### 📄 **appsettings.json** - Store Configuration

**Real-Life Analogy**: ⚙️ **Store Settings & Rules Book**

Like a file that says:
- Store hours: 9am - 9pm
- Music volume: Medium
- Temperature: 72°F
- Security camera recording level: High

**What it contains**:
```json
{
  "Logging": {                    // 📹 Security camera settings
    "LogLevel": {
      "Default": "Information",    // Record everything important
      "Microsoft.AspNetCore": "Warning"  // Only record problems
    }
  },
  "AllowedHosts": "*"             // 🌍 Accept customers from anywhere
}
```

---

#### 📄 **HRBackendExercise.API.csproj** - Building Instructions

**Real-Life Analogy**: 📐 **Architect's Blueprint**

Tells the construction crew (compiler):
- What version of .NET to use (.NET 8.0)
- What tools/packages to include (Swagger, etc.)
- Build settings

---

### 🔍 **HRBackendExercise.API.Tests** - The Quality Inspector

**Real-Life Analogy**: 🔬 **Health & Safety Inspector**

Before opening the store, an inspector comes to verify:
- Does the fire alarm work? ✅
- Are the emergency exits clear? ✅
- Does the cash register calculate correctly? ✅
- Can customers actually buy products? ✅

#### 📁 **Controllers/ProductsControllerTests.cs**

**Testing scenarios**:
```
Inspector: "Let me test the counter staff..."

Test 1: "What happens if I ask for all products?"
✅ Should give me a list

Test 2: "What if I ask for product #999 that doesn't exist?"
✅ Should tell me "Not Found"

Test 3: "What if I try to add a product with no price?"
✅ Should reject it with an error message

Test 4: "What if I update a product correctly?"
✅ Should confirm the update
```

#### 📁 **Services/ProductServiceTests.cs**

**Testing the manager**:
```
Inspector: "Let me test the store manager..."

Test 1: "Can they add a new product correctly?"
✅ Should assign an ID and store it

Test 2: "What if they try to add a product with price = $0?"
✅ Should reject it (price must be > 0)

Test 3: "Can they find a product by ID?"
✅ Should return the correct product

Test 4: "What if they delete a product?"
✅ Should remove it from inventory
```

**Why testing is important**: You wouldn't open a store without checking if the cash register works, right? Same here.

---

### 📜 **HRBackendExercise.sln** - The Building Blueprint

**Real-Life Analogy**: 🗺️ **Mall Master Plan**

This file tells Visual Studio:
- "This mall has 2 buildings (projects)"
- "Here's how they relate to each other"
- "Here's how to build both of them together"

---

## 🎨 Architecture Explained - The Restaurant Analogy

Let's use a **restaurant** to explain the architecture:

```
CUSTOMER EXPERIENCE AT A RESTAURANT:

1. 🚶 Customer walks in (HTTP Request arrives)
   ↓
2. 🎫 Host at the front desk (Controller)
   - Greets customer
   - Takes order
   - Validates: "Do you want menu item #5? Let me check..."
   ↓
3. 👔 Manager/Chef (Service)
   - Receives order from host
   - Checks inventory
   - Prepares the food (business logic)
   - Validates: "Do we have ingredients? Is this recipe valid?"
   ↓
4. 📦 Pantry/Inventory (In-Memory List)
   - Stores all ingredients (products)
   ↓
5. 🍽️ Food is ready
   ↓
6. 🎫 Host delivers to customer (Controller returns response)
   - "Here's your meal!" (200 OK + product data)
   - "Sorry, we're out of that" (404 Not Found)
   - "That's not on our menu" (400 Bad Request)
```

**Why this separation?**
- **Host** (Controller) is good at talking to customers, not cooking
- **Chef** (Service) is good at cooking, not customer service
- **Pantry** (Data Storage) just holds ingredients
- Each person does what they're best at!

---

## 🏗️ Design Patterns Used (Simplified)

### 1. **Dependency Injection** 🏠
**Analogy**: Moving into a furnished apartment
- You don't buy furniture yourself
- The apartment comes with everything you need
- Just move in and use it!

### 2. **Repository/Service Pattern** 🏦
**Analogy**: Bank teller vs. vault
- You don't go into the vault yourself
- The teller (service) handles access to money (data)
- Safe and organized!

### 3. **Singleton Pattern** 👑
**Analogy**: One president per country
- Only ONE store manager for the whole store
- Everyone talks to the same manager
- Shares the same inventory list

---

## 🛠 Prerequisites

- .NET 8.0 SDK or later
- Visual Studio 2022 or VS Code (optional)

---

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

### 5. Run tests

```bash
cd HRBackendExercise.API.Tests
dotnet test
```

---

## 📚 API Endpoints - How Customers Interact

Think of these as **different windows at a service desk**:

### 🪟 **Window 1: "Show me all products"**
```http
GET /api/products
```

**Real-Life**: Walking into a store and asking "What do you have in stock?"

**Response**: Staff shows you the complete catalog
```json
[
    {
        "id": 1,
        "sku": "PROD-001",
        "description": "Blue T-Shirt",
        "price": 19.99
    },
    {
        "id": 2,
        "sku": "PROD-002",
        "description": "Red Hat",
        "price": 12.50
    }
]
```

---

### 🪟 **Window 2: "Show me product #5"**
```http
GET /api/products/5
```

**Real-Life**: "Can I see the details of item #5?"

**Response**: Staff shows you that specific item
```json
{
    "id": 5,
    "sku": "PROD-005",
    "description": "Green Sneakers",
    "price": 89.99
}
```

**If product doesn't exist**: "Sorry, we don't have item #5" (404 Not Found)

---

### 🪟 **Window 3: "Add this new product to inventory"**
```http
POST /api/products
```

**Real-Life**: Manager brings a new product to add to the store

**You provide**:
```json
{
    "sku": "SHIRT-BLU-XL",
    "description": "Blue XL Cotton Shirt",
    "price": 29.99
}
```

**Staff assigns it a barcode and adds it**:
```json
{
    "id": 10,
    "sku": "SHIRT-BLU-XL",
    "description": "Blue XL Cotton Shirt",
    "price": 29.99
}
```

**Rejections**:
- "You forgot the SKU!" → 400 Bad Request
- "Price can't be $0!" → 400 Bad Request

---

### 🪟 **Window 4: "Update product #3"**
```http
PUT /api/products/3
```

**Real-Life**: "I need to change the price of item #3"

**You provide updated info**:
```json
{
    "sku": "PROD-003",
    "description": "Updated Description",
    "price": 39.99
}
```

**Response**: "Done!" (204 No Content)

**If product doesn't exist**: "Can't update item #999, it doesn't exist!" (404 Not Found)

---

### 🪟 **Window 5: "Remove product #7"**
```http
DELETE /api/products/7
```

**Real-Life**: "Take item #7 off the shelves"

**Response**: "Removed!" (204 No Content)

**If product doesn't exist**: "Can't delete item #999, it doesn't exist!" (404 Not Found)

---

## 🧪 Testing - The Inspector's Checklist

**Real-Life**: Before opening day, an inspector tests EVERYTHING

### Run all tests

```bash
cd HRBackendExercise.API.Tests
dotnet test
```

### Run tests with detailed output

```bash
dotnet test --verbosity detailed
```

### Run tests with coverage:

```bash
dotnet test /p:CollectCoverage=true
```

### Test Categories:

#### 1️⃣ **Controller Tests** (Testing the Counter Staff)
```
Scenario: Customer asks for all products
✅ Test: Does staff give them a list?

Scenario: Customer asks for product #999 (doesn't exist)
✅ Test: Does staff say "Not Found"?

Scenario: Customer tries to add a product with no price
✅ Test: Does staff reject it?

Scenario: Customer provides valid product data
✅ Test: Does staff accept it and return confirmation?
```

#### 2️⃣ **Service Tests** (Testing the Manager)
```
Scenario: Manager adds a new product
✅ Test: Is it stored correctly?
✅ Test: Does it get a unique ID?

Scenario: Manager tries to add product with $0 price
✅ Test: Does manager reject it?

Scenario: Manager looks up product by ID
✅ Test: Does manager find the right one?

Scenario: Manager updates a product
✅ Test: Are changes saved?

Scenario: Manager deletes a product
✅ Test: Is it really gone?
```

---

## 🏗 Architecture - The Complete Picture

### **The 3-Layer Cake** 🎂

```
┌─────────────────────────────────────┐
│  🎫 COUNTER STAFF (Controllers)     │  ← Talk to customers
│     "How can I help you?"            │  ← Handle HTTP requests
├─────────────────────────────────────┤
│  👔 STORE MANAGER (Services)        │  ← Business logic
│     "Let me check the inventory"    │  ← Validate & process
├─────────────────────────────────────┤
│  📦 WAREHOUSE (In-Memory List)      │  ← Store products
│     "All products stored here"      │  ← Data storage
└─────────────────────────────────────┘
```

**Why layers?**
- Counter staff don't manage inventory (separation of concerns)
- Manager can be replaced without retraining counter staff (flexibility)
- Each layer has ONE job and does it well (single responsibility)

---

## 🎯 Key Features Explained Simply

### ✅ **Input Validation** - The Bouncer

**Real-Life**: Like a bouncer checking IDs at a club

```
❌ "You can't enter without a name" → SKU is required
❌ "Price must be more than $0" → Price validation
❌ "That's not valid information" → Data validation
✅ "Everything looks good, come in!" → Validation passed
```

### 🚨 **Error Handling** - Clear Communication

**Real-Life**: Instead of just saying "NO", we explain WHY

```
🟢 200 OK → "Here's what you asked for"
🟢 201 Created → "Successfully added"
🟢 204 No Content → "Successfully updated/deleted"
🔴 400 Bad Request → "Your request has errors: [details]"
🔴 404 Not Found → "That product doesn't exist"
🔴 500 Server Error → "Oops, something broke on our end"
```

### 📖 **Swagger Documentation** - Interactive Manual

**Real-Life**: Like a menu at a restaurant with pictures

- See all available actions (endpoints)
- Try them out directly in your browser
- See example requests and responses
- No need to memorize commands!

**Access it**: Open your browser → `https://localhost:5001/swagger`

---

## 🏗 Technologies Used

- **ASP.NET Core 8.0** - Web API framework
- **Swagger/OpenAPI** - API documentation
- **xUnit** - Testing framework
- **Moq** - Mocking library for unit tests

---

## 📋 Important Notes for Non-Programmers

### 💾 **In-Memory Storage** - The Notepad
**What it means**: Products are stored in the computer's temporary memory (RAM), like writing on a notepad.

**The catch**: When you close the application, the notepad is thrown away. All products disappear! 📝❌

**In real life**: You'd use a database (like a filing cabinet) to keep data permanently.

---

### 👥 **Singleton Pattern** - One Manager for Everyone
**What it means**: Everyone uses the SAME product list.

**Example**:
- Person A adds a product → It appears for everyone
- Person B sees all products Person A added
- Everyone shares the same inventory

**In real life**: If this were a multi-store chain, each store would have its own manager (different pattern).

---

### 🔌 **No Database** - This is a Demo
**What it means**: This is a simplified learning project.

**In production**:
- ✅ Would connect to SQL Server / PostgreSQL
- ✅ Data would persist (survive restarts)
- ✅ Handle millions of products
- ✅ Multiple users simultaneously

---

## 🚀 What's Missing (Future Enhancements)

Think of these as "planned renovations for the store":

- [ ] 💾 **Database**: Permanent storage (not just a notepad)
- [ ] 🔐 **Security**: Login system (authentication)
- [ ] 📄 **Pagination**: Show 10 products at a time (not all 10,000!)
- [ ] 🔍 **Search**: "Find all blue shirts under $30"
- [ ] 📊 **Logging**: Detailed activity logs (Serilog)
- [ ] 🐳 **Docker**: Package the app in a container

---

## 🎓 What You Learned (Even as a Non-Programmer)

1. **APIs are like service counters** - You ask, they respond
2. **Layers separate responsibilities** - Counter staff ≠ Warehouse manager
3. **Validation protects the system** - Bouncers check IDs
4. **Testing ensures quality** - Inspectors verify everything works
5. **Dependency Injection is like getting a furnished apartment** - Don't build, receive!
6. **Design patterns are proven solutions** - Like using IKEA instructions instead of inventing furniture

---

## 📄 License

This project is created as a coding exercise and is open source for educational purposes.

---

## 👤 Author

**Edgar Arturo Martinez**
- GitHub: [@EdgarArturoMartinez](https://github.com/EdgarArturoMartinez)

---

## 📞 Support

If you're a non-programmer and got this far: **Congratulations!** 🎉 

You now understand more about software architecture than most people!
