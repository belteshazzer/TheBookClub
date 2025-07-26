# 📚 TheBookClub

**TheBookClub** TheBookClub is an **ASP.NET Core Web API** designed to manage book-related operations, user interactions, and real-time notifications. It features **user authentication and authorization** with role-based access control, **two-factor authentication (2FA)** for enhanced security, and **JWT-based authentication** for secure API access. The application supports CRUD operations for books, authors, genres, and reviews, along with **file uploads** for book-related content.

Key features include **real-time notifications** using SignalR, allowing users to receive updates on new books or events, and **soft delete functionality** for preserving data integrity. The API also provides endpoints for managing bookmarks, personalized recommendations, and administrative tools for managing roles and users. Designed with scalability and security in mind, TheBookClub is a robust backend solution for book management systems..

---

## 🚀 Features

### 🔐 User Authentication and Authorization
- Role-based access control (`Admin`, `User`)
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

### Other Features
- File download support for book content.
- Resend email verification functionality.
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
- Encodings.Web for QR code generation

---


## ⚙️ Setup and Configuration

### ✅ Prerequisites
- [.NET 8.0 SDK](https://dotnet.microsoft.com/download)
- SQL Server
- Visual Studio 2022 or VS Code

### 🧪 How to Fork and Run
```
1. Clone the repository
git clone https://github.com/your-username/TheBookClub.git

2. Install dependencies by running the following
dotnet restore

3. create your database on SQL
4. Configure database in appsettings.json
  "ConnectionStrings": {
  "DefaultConnection": "Server=YOUR_SERVER;Database=TheBookClubDB;Trusted_Connection=True;"
}
5. Apply migrations
dotnet ef database update

6. Run the application
dotnet run
```
Access Swagger UI at:
👉 https://localhost:5001/swagger

### 🔑 Key API Endpoints
## 🔐 Authentication
`POST /api/auth/login` - Login and receive JWT

`POST /api/auth/register` - Register a new user

## 📚 Books
`GET /api/books` - Get all books

`POST /api/books` - Add new book (Admin only)

`DELETE /api/books/{id}` - Soft delete (Admin only)

## 🔔 Notifications
`POST /api/notifications/SendNotification/{userId}` - Send to a user

`POST /api/notifications/SendGroupNotification` - Send to a group

## 👥 Roles
`GET /api/roles` - Get all roles (Admin only)

`POST /api/roles` - Add a new role (Admin only)

### 🚀 Advanced Features
- Real-Time Notifications to a user and group of users using SignalR

- Role-Based Authorization: [Authorize(Roles = "Admin,User")]

- Persistent Notifications for offline users


 ### 🌱 Future Enhancements
- Social login support (Google, Facebook)

- ML.NET-powered recommendation engine

- Localization for multiple languages

- Frontend client using React or Angular

  ### 📬 Contact
Email: daniyirdaw0310@gmail.com
