# 📚 OnlineBookstoreAPI

## 📖 Overview
OnlineBookstoreAPI is a RESTful API built with ASP.NET Core that powers an online bookstore system. It provides endpoints for managing books, authors, customers, and orders while supporting secure authentication and authorization.

---

## 🚀 Features
- **ASP.NET Core Web API** architecture.  
- **Entity Framework Core** for database interactions.  
- **CRUD operations** for books, authors, and orders.  
- **User authentication & authorization** with JWT tokens.  
- **Order management** with cart and checkout support.  
- **Pagination & filtering** for large datasets.  
- **Swagger / OpenAPI** documentation.  
- **Error handling & logging** middleware.  

---

## 🛠️ Tech Stack
- **Backend:** ASP.NET Core 7.0 Web API  
- **Database:** SQL Server (Entity Framework Core)  
- **Authentication:** JWT-based Authentication  
- **Documentation:** Swagger (Swashbuckle)  
- **Testing:** xUnit / NUnit  

---

## 📂 Project Structure
```
OnlineBookstoreAPI/
│-- Controllers/       # API controllers
│-- Models/            # Entity & DTO models
│-- Data/              # DbContext & Migrations
│-- Repositories/      # Data access layer
│-- Services/          # Business logic
│-- Middlewares/       # Custom middlewares
│-- Program.cs         # Entry point
│-- appsettings.json   # Configurations
```

---

## ⚙️ Getting Started

### Prerequisites
- .NET 7.0 SDK  
- SQL Server / LocalDB  
- Visual Studio 2022 or VS Code  

### Installation
1. Clone the repository:
   ```bash
   git clone https://github.com/Anish2944/OnlineBookstoreAPI.git
   cd OnlineBookstoreAPI
   ```
2. Update the database connection string in `appsettings.json`.
3. Apply migrations:
   ```bash
   dotnet ef database update
   ```
4. Run the project:
   ```bash
   dotnet run
   ```

---

## 📦 API Documentation
- Swagger UI is available at:  
  ```
  https://localhost:5001/swagger
  ```

---

## ✅ Testing
Run unit tests using:
```bash
dotnet test
```

---

## 🤝 Contributing
1. Fork the repo  
2. Create a feature branch (`git checkout -b feature/YourFeature`)  
3. Commit changes (`git commit -m "Add feature"`)  
4. Push (`git push origin feature/YourFeature`)  
5. Open a Pull Request  

---

## 📜 License
This project is licensed under the MIT License - see the [LICENSE](LICENSE) file for details.  
