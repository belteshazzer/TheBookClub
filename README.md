# 📚 TheBookClub

**TheBookClub** is a feature-rich **ASP.NET Core Web API** project designed to manage E-Book operations, user interactions, and real-time notifications. This project demonstrates advanced concepts in ASP.NET Core, including **role-based authorization**, **real-time communication using SignalR**, and a **robust notification system**.

---

## 🚀 Features

### 🔐 User Authentication and Authorization
- Role-based access control (`Admin`, `Moderator`, `User`)
- JWT-based authentication for secure API access
- Two-factor authentication (2FA) for enhanced security

### 📚 Book Management
- CRUD operations for books, authors, and genres
- Soft delete functionality for books
- Real-time notifications for new book additions

### 🔖 Bookmarking and Recommendations
- Users can bookmark genres and receive notifications
- Personalized book recommendations based on preferences

### 🔔 Real-Time Notifications
- Notifications for users and groups via **SignalR**
- Persistent notifications stored in the database

### 🔍 Advanced Search and Filtering
- Full-text search for books, authors, and genres
- Filter/sort by rating, genre, and publication date

### 📊 Admin Dashboard
- Analytics for total users, books, and reviews
- Role management and user account control

---

## 🛠 Technologies Used

### ✅ Backend
- ASP.NET Core 8.0
- Entity Framework Core 9.0.3 (with SQL Server)
- SignalR for real-time communication
- AutoMapper for object mapping

### ✅ Authentication and Authorization
- ASP.NET Core Identity
- JWT Bearer Authentication

### ✅ Database
- Microsoft SQL Server

### ✅ Other Libraries
- Swashbuckle (Swagger) for API documentation
- Microsoft.Extensions.Logging for logging
- Newtonsoft.Json for JSON serialization

---

## 📂 Project Structure

/Controllers -> API endpoints /Services -> Business logic /Repositories -> Data access with EF Core /Hubs -> SignalR hubs /Models -> Entity models /Configurations -> App settings and configuration


---

## ⚙️ Setup and Configuration

### ✅ Prerequisites
- [.NET 8.0 SDK](https://dotnet.microsoft.com/download)
- SQL Server
- Visual Studio 2022 or VS Code

### 📦 Install Dependencies
```bash
dotnet restore
```
### 🔧 Configuration

## Database Connection

Edit appsettings.json:
```
"ConnectionStrings": {
  "DefaultConnection": "Server=YOUR_SERVER;Database=TheBookClubDB;Trusted_Connection=True;"
}
// JWT Configuration
"Jwt": {
  "Key": "YourSecretKeyHere",
  "Issuer": "yourapp.com",
  "Audience": "yourapp.com"
}

```
### 📦 Database Migrations
```
dotnet ef database update
```

### Running the Project
```
dotnet run
```

Access Swagger UI at:
👉 https://localhost:5001/swagger

### 🧪 How to Fork and Run
```
1. Clone the repository
git clone https://github.com/your-username/TheBookClub.git

2. Install dependencies
dotnet restore

3. Configure database in appsettings.json

4. Apply migrations
dotnet ef database update

5. Run the application
dotnet run
```

### 🔑 Key API Endpoints
## 🔐 Authentication
`POST /api/auth/login` - Login and receive JWT

`POST /api/auth/register` - Register a new user

## 📚 Books
`GET /api/books` - Get all books

`POST /api/books` - Add new book (Admin only)

`DELETE /api/books/{id}` - Soft delete (Admin only)

## 🔔 Notifications
`POST /api/notifications/send` - Send to a user

`POST /api/notifications/send-group` - Send to a group

## 👥 Roles
`GET /api/roles` - Get all roles (Admin only)

`POST /api/roles` - Add a new role (Admin only)

### 🚀 Advanced Features
- Real-Time Notifications using SignalR

- Role-Based Authorization: [Authorize(Roles = "Admin,Moderator")]

- Persistent Notifications for offline users

- Group Notifications via SignalR groups

 ### 🌱 Future Enhancements
- Social login support (Google, Facebook)

- ML.NET-powered recommendation engine

- Localization for multiple languages

- Frontend client using React or Angular

  ### 📬 Contact
Email: daniyirdaw0310@gmail.com
